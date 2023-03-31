using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;
using NetProtocol;

namespace GameCore
{
    [Module]
    public class LogicTickMgr:Module
    {
        public int SFrameId { get; private set; }
        public int CFrameId { get; private set; }

        //本地预测的指令
        //帧数，操作指令队列
        private Dictionary<int, Queue<OpKey>> forecastOpKeyDict = new Dictionary<int, Queue<OpKey>>();


        /// <summary>
        /// 开始客户端逻辑帧轮询
        /// </summary>
        /// <param name="frameStartTime"></param>
        public void StartClientFrame(long frameStartTime)
        {
            GetModule<TimerManager>().AddMsTickTimerTask(66, ClientLogicTick, null, 0, frameStartTime);
        }



        private void ClientLogicTick()
        {
            CFrameId++;
            GameEvent.LockStep.ClientFrameChange?.Invoke(CFrameId);
            //对所有帧同步行为进行预测
        }
        private void ServerLogicTick(int sFrameId)
        {
            //Tick Hero
            for (int i = 0; i < heroList.Count; i++)
            {
                heroList[i].LogicTick();
            }
        }
    }
}
