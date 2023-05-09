using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;
using NetProtocol;
using UnityEngine;

namespace GameCore
{
    [Module]
    public class LogicTickMgr : Module
    {
        public const int LogicTickInerval = 66;
        public const int MaxAheadFrame = 10;
        public const int MinAheadFrame = 2;
        public int SFrameId { get; private set; }
        public int CFrameId { get; private set; }
        //开始计时后固定频率更新的帧数，作为本地客户端帧速率变化的参考
        public int RealFrameId { get; private set; }

        protected bool canSendOpKey = false;
        private bool isClientLogicStart = false;
        //客户端领先服务器的帧数，可动态变化
        private int m_aheadFrame = 3;
        private int m_clientTickTimerId;
        private int m_realTickTimerId;
        private bool isExpandInterval = false;
        private bool isReduceInterval = false;
        private int speedChangeTgtFrame = 0;
        private int speedChangeIndex = 0;


        public override void OnUpdate()
        {
            ViewTick();
        }
        /// <summary>
        /// 开始客户端逻辑帧轮询
        /// </summary>
        /// <param name="frameStartTime"></param>
        public void StartClientFrame(long frameStartTime)
        {
            if (isClientLogicStart)
            {
                return;
            }
            isClientLogicStart = true;
            canSendOpKey = true;
            //客户端提前3帧开始
            CFrameId = 3;
            m_clientTickTimerId = GetModule<TimerManager>().AddMsTickTimerTask(LogicTickInerval, ClientLogicTick, null, 0, frameStartTime);
            m_realTickTimerId = GetModule<TimerManager>().AddMsTickTimerTask(LogicTickInerval, RealLogicTick, null, 0, frameStartTime);
        }
        /// <summary>
        /// 收到服务器帧消息并进行校验
        /// 如果与预测帧不一样，则回滚到前一帧，执行服务器帧，接着再预测快速执行到客户端当前帧
        /// </summary>
        /// <param name="msg"></param>
        public void CheckOutOpKey(S2COpKeyMsg msg)
        {
            SFrameId = msg.FrameId;
            GameEvent.LockStep.ServerFrameChange?.Invoke(SFrameId);
            ISyncUnit unit;
            OpKey opKey;
            //检测是否不一致
            var allSyncUnits = GetModule<GameUnitMgr>().GetAllSyncUnits();
            var remainSyncUnits = allSyncUnits.Select(t => t.NetUrl).ToList();
            List<OpKey> differenceOpkeys = new List<OpKey>();
            for (int i = 0; i < msg.OpKeyList.Count; i++)
            {
                opKey = msg.OpKeyList[i];
                unit = GetModule<GameUnitMgr>().GetSyncUnit(opKey.PlayerId);
                remainSyncUnits.Remove(opKey.PlayerId);
                if (unit != null)
                {
                    if (!unit.CheckOutOpKey(SFrameId, opKey))
                    {
                        differenceOpkeys.Add(opKey);
                    }
                }
                else
                {
                    Debug.LogError($"不存在单位{opKey.PlayerId}");
                }
            }
            //没有指令的单位按空指令检测
            for (int i = 0; i < remainSyncUnits.Count; i++)
            {
                unit = GetModule<GameUnitMgr>().GetSyncUnit(remainSyncUnits[i]);
                opKey = new OpKey() { KeyType = OpKeyType.None, PlayerId = remainSyncUnits[i] };
                if (!unit.CheckOutOpKey(SFrameId, opKey))
                {
                    differenceOpkeys.Add(opKey);
                }
            }
            //全部一致，清除所有SyncUnit那一帧的缓存
            if (differenceOpkeys.Count == 0)
            {
                for (int i = 0; i < allSyncUnits.Count; i++)
                {
                    allSyncUnits[i].ReleaseOperateInfo(SFrameId);
                }
            }
            //替换消息中玩家的那一帧
            else
            {
                for (int i = 0; i < differenceOpkeys.Count; i++)
                {
                    opKey = differenceOpkeys[i];
                    unit = GetModule<GameUnitMgr>().GetSyncUnit(opKey.PlayerId);
                    if (unit != null)
                    {
                        unit.ReplaceOperateInfo(SFrameId, opKey);
                    }
                }
                //回滚到前一帧
                Debug.LogError($"{CFrameId},RollBackTo:{SFrameId - 1}");
                GetModule<RollBackMgr>().RollBackTo(SFrameId - 1);
                //追赶到当前客户端帧
                ChaseFrame(SFrameId, false);
            }
        }
        /// <summary>
        /// 每帧记录玩家指令，但每个逻辑帧只发送一次有效指令
        /// </summary>
        public void SendOpKey(OperateInfo operateInfo)
        {
            if (canSendOpKey == false)
            {
                ReferencePool.Release(operateInfo);
                return;
            }
            canSendOpKey = false;
            OpKey opKey = new OpKey();
            if (operateInfo.KeyType == OpKeyType.Move)
            {
                MoveKey moveKey = new MoveKey();
                moveKey.X_Value = operateInfo.InputDir.x.ScaledValue;
                moveKey.Z_Value = operateInfo.InputDir.z.ScaledValue;
                opKey.MoveKey = moveKey;
                opKey.KeyType = OpKeyType.Move;
                if (operateInfo.InputDir == FXVector3.zero)
                {
                    operateInfo.KeyType = OpKeyType.None;
                }
            }
            if (operateInfo.KeyType != OpKeyType.None)
            {
                GetModule<NetWorkMgr>().SendMsg(NetCmd.C2SOpKey, new C2SOpKeyMsg() { FrameId = CFrameId, OpKey = opKey });
            }
            GetModule<GameUnitMgr>().MainHero.InputKey(operateInfo);
            GetModule<GameUnitMgr>().MainHero.CacheOperate(CFrameId, operateInfo);
        }
        public void ChangeAheadFrame(int aheadFrame)
        {
            //本地必须领先服务器
            if (aheadFrame <= 0 || aheadFrame == m_aheadFrame)
            {
                return;
            }

            if (aheadFrame > m_aheadFrame)
            {
                //加快客户端逻辑帧运行速率
                isExpandInterval = true;
                //计算加速时每帧间隔
                var offsetFrame = Mathf.Abs(m_aheadFrame - aheadFrame);
                //变速需在n个真实帧内完成
                int n = 1;
                int interval = LogicTickInerval * n / (offsetFrame + n);
                speedChangeTgtFrame = RealFrameId + n + aheadFrame;

                GetModule<TimerManager>().ChangeMsTickTimerTaskInterval(m_clientTickTimerId, interval);
            }
            else
            {
                //暂停客户端逻辑帧运行
                isReduceInterval = true;
                GetModule<TimerManager>().Pause(m_clientTickTimerId);
            }
            m_aheadFrame = aheadFrame;

        }

        private void ChaseFrame(int frameId, bool isForcast)
        {
            //其他玩家操作单位重新预测并输入，本地玩家将旧的操作指令输入
            var mainHero = GetModule<GameUnitMgr>().MainHero;
            var heroList = GetModule<GameUnitMgr>().GetEntitiyByType(EntityType.Hero);
            for (int i = 0; i < heroList.Count; i++)
            {
                var hero = (heroList[i] as HeroEntity);
                if (hero.NetUrl != mainHero.NetUrl)
                {
                    if (isForcast)
                    {
                        var operateInfo = hero.ForcastOpkey(frameId);
                        hero.CacheOperate(frameId, operateInfo);
                    }
                    else
                    {
                        var operateInfo = hero.GetOperateInfo(frameId);
                        hero.InputKey(operateInfo);
                    }
                }
                else
                {
                    var operateInfo = hero.GetOperateInfo(frameId);
                    hero.InputKey(operateInfo);
                }
            }

            EntityClientLogicTick(frameId);

            if (frameId == CFrameId - 1)
            {
                //追赶结束
                return;
            }
            else
            {
                ChaseFrame(frameId + 1, true);
            }
        }

        //根据玩家的输入和预测运行各个帧同步实体
        private void ClientLogicTick()
        {
            CFrameId++;

            //在帧结束的时候执行操作
            canSendOpKey = true;
            GameEvent.LockStep.ClientFrameChange?.Invoke(CFrameId);
            //对可操作单位所有帧同步行为进行预测
            var mainHero = GetModule<GameUnitMgr>().MainHero;
            var heroList = GetModule<GameUnitMgr>().GetEntitiyByType(EntityType.Hero);
            for (int i = 0; i < heroList.Count; i++)
            {
                var hero = (heroList[i] as HeroEntity);
                //本地玩家操作英雄不参与预测
                if (hero.NetUrl != mainHero.NetUrl)
                {
                    var operateInfo = hero.ForcastOpkey(CFrameId);
                    hero.CacheOperate(CFrameId, operateInfo);
                }
            }

            EntityClientLogicTick(CFrameId);

            if (speedChangeTgtFrame == CFrameId && isExpandInterval)
            {
                Debug.Log("速率变动结束");
                isExpandInterval = false;
                GetModule<TimerManager>().ChangeMsTickTimerTaskInterval(m_clientTickTimerId, LogicTickInerval);
            }
        }
        private void EntityClientLogicTick(int frameId)
        {
            var heroList = GetModule<GameUnitMgr>().GetEntitiyByType(EntityType.Hero);
            //所有帧同步单位执行行为
            //1.英雄
            for (int i = 0; i < heroList.Count; i++)
            {
                (heroList[i] as LogicEntity).ClientLogicTick(frameId);
            }
            //逻辑帧结束进行快照
            GetModule<RollBackMgr>().TakeSnapShot(frameId);
        }

        private void RealLogicTick()
        {
            RealFrameId++;

            GameEvent.LockStep.RealFrameChange?.Invoke(RealFrameId);

            ////结束会导致客户端逻辑帧变速时回退过久，执行多次
            //if (RealFrameId + m_aheadFrame == CFrameId && isChangeInterval)
            //{
            //    Debug.Log("速率变动结束");
            //    isChangeInterval = false;
            //    GetModule<TimerManager>().ChangeMsTickTimerTaskInterval(m_clientTickTimerId, LogicTickInerval);
            //    logInfo2 = true;
            //    endframe1 = RealFrameId;
            //}
            if (RealFrameId + m_aheadFrame == CFrameId && isReduceInterval)
            {
                Debug.Log("暂停结束");
                isReduceInterval = false;
                GetModule<TimerManager>().Continue(m_clientTickTimerId);
            }
        }

        //表现层刷新
        private void ViewTick()
        {
            //英雄刷新帧
            var heroList = GetModule<GameUnitMgr>().GetEntitiyByType(EntityType.Hero);
            for (int i = 0; i < heroList.Count; i++)
            {
                (heroList[i] as HeroEntity).ViewTick();
            }
        }
    }
}
