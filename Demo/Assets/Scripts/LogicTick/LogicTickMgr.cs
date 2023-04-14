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
        public int SFrameId { get; private set; }
        public int CFrameId { get; private set; }

        protected bool canSendOpKey = false;


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
            canSendOpKey = true;
            GetModule<TimerManager>().AddMsTickTimerTask(66, ClientLogicTick, null, 0, frameStartTime);
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
            bool isEqual = true;
            for (int i = 0; i < msg.OpKeyList.Count; i++)
            {
                opKey = msg.OpKeyList[i];
                unit = GetModule<GameUnitMgr>().GetSyncUnit(opKey.PlayerId);
                if (unit != null)
                {
                    isEqual &= unit.CheckOutOpKey(SFrameId, opKey);
                }
                else
                {
                    Debug.LogError($"不存在单位{opKey.PlayerId}");
                }
            }
            //全部一致，清除所有SyncUnit那一帧的缓存
            if (isEqual)
            {
                var allSyncUnits = GetModule<GameUnitMgr>().GetAllSyncUnits();
                for (int i = 0; i < allSyncUnits.Count; i++)
                {
                    allSyncUnits[i].ReleaseOperateInfo(SFrameId);
                }
            }
            //替换消息中玩家的那一帧
            else
            {
                for (int i = 0; i < msg.OpKeyList.Count; i++)
                {
                    opKey = msg.OpKeyList[i];
                    unit = GetModule<GameUnitMgr>().GetSyncUnit(opKey.PlayerId);
                    if (unit != null)
                    {
                        unit.ReplaceOperateInfo(SFrameId, opKey);
                    }
                }
                //回滚到前一帧
                GetModule<RollBackMgr>().RollBackTo(SFrameId - 1);
                //追赶到当前客户端帧
                ChaseFrame(SFrameId);
            }
        }
        //todo 告知服务器是哪一帧消息，等服务器执行到对应帧再返回
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
            OpKey opKey = new OpKey();
            if (operateInfo.KeyType == OpKeyType.Move)
            {
                MoveKey moveKey = new MoveKey();
                moveKey.X_Value = operateInfo.InputDir.x.ScaledValue;
                moveKey.Z_Value = operateInfo.InputDir.z.ScaledValue;
                opKey.MoveKey = moveKey;
                opKey.KeyType = OpKeyType.Move;
            }
            GetModule<NetWorkMgr>().SendMsg(NetCmd.C2SOpKey, new C2SOpKeyMsg() { OpKey = opKey });
            GetModule<GameUnitMgr>().MainHero.InputKey(operateInfo);
        }

        private void ChaseFrame(int frameId)
        {
            //其他玩家操作单位重新预测并输入，本地玩家将旧的操作指令输入
            var mainHero = GetModule<GameUnitMgr>().MainHero;
            var heroList = GetModule<GameUnitMgr>().GetEntitiyByType(EntityType.Hero);
            for (int i = 0; i < heroList.Count; i++)
            {
                var hero = (heroList[i] as HeroEntity);
                if (hero.NetUrl != mainHero.NetUrl)
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

            EntityClientLogicTick();

            if (frameId == CFrameId)
            {
                //追赶结束
                return;
            }
            else
            {
                ChaseFrame(frameId + 1);
            }
        }

        //根据玩家的输入和预测运行各个帧同步实体
        private void ClientLogicTick()
        {
            CFrameId++;
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

            EntityClientLogicTick();

            //逻辑帧结束进行快照
            GetModule<RollBackMgr>().TakeSnapShot();
        }
        private void EntityClientLogicTick()
        {
            var heroList = GetModule<GameUnitMgr>().GetEntitiyByType(EntityType.Hero);
            //所有帧同步单位执行行为
            //1.英雄
            for (int i = 0; i < heroList.Count; i++)
            {
                (heroList[i] as LogicEntity).ClientLogicTick();
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
