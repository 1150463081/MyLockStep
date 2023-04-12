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
            GetModule<TimerManager>().AddMsTickTimerTask(66, ClientLogicTick, null, 0, frameStartTime);
        }
        /// <summary>
        /// 收到服务器帧消息并进行校验
        /// </summary>
        /// <param name="msg"></param>
        public void CheckOutOpKey(S2COpKeyMsg msg)
        {
            SFrameId = msg.FrameId;
            GameEvent.LockStep.ServerFrameChange?.Invoke(SFrameId);
            ISyncUnit unit;
            OpKey opKey;
            for (int i = 0; i < msg.OpKeyList.Count; i++)
            {
                opKey = msg.OpKeyList[i];
                unit = GetModule<GameUnitMgr>().GetSyncUnit(opKey.PlayerId);
                if (unit != null)
                {
                    //unit.InputKey(opKey);
                }
                else
                {
                    Debug.LogError($"不存在单位{opKey.PlayerId}");
                }
            }

            ServerLogicTick(SFrameId);
        }


        //根据玩家的输入和预测运行各个帧同步实体
        private void ClientLogicTick()
        {
            CFrameId++;
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
                    hero.ForcastOpkey();
                }
            }
            //所有帧同步单位执行行为
            //1.英雄
            for (int i = 0; i < heroList.Count; i++)
            {
                (heroList[i] as LogicEntity).ClientLogicTick();
            }


            //逻辑帧结束进行快照
            GetModule<RollBackMgr>().TakeSnapShot();
        }
        //服务器帧运行，对已执行帧进行检测，回滚，追赶操作
        private void ServerLogicTick(int sFrameId)
        {
            var heroList = GetModule<GameUnitMgr>().GetEntitiyByType(EntityType.Hero);
            
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
