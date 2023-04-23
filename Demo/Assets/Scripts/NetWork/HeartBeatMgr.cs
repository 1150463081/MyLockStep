﻿using NetProtocol;
using LockStepFrame;
using System.Collections.Generic;

namespace GameCore
{
    [Module]
    public class HeartBeatMgr : Module
    {
        /// <summary>
        /// 网络延时
        /// </summary>
        public long NetDelay { get; private set; }

        private int delay = 500;
        private int sendIndex = 0;
        private int cacheCnt = 10;
        private Queue<long> timeQueue=new Queue<long>();
        private long timeSum;//所有延时时间和

        public override void OnStart()
        {
            StartHeartBeat();
        }
        public void StartHeartBeat()
        {
            GetModule<TimerManager>().AddMsTickTimerTask(delay, SendHeartBeat, null, 0);
        }
        public void ReciveHeartBeat(S2CHeartBeatMsg msg)
        {
            if (timeQueue.Count > cacheCnt)
            {
                var time = timeQueue.Dequeue();
                timeSum -= time;
            }
            var nowTime = Utility.Time.MillisecondNow();
            var offest = nowTime - msg.SendTime;
            timeQueue.Enqueue(offest);
            timeSum += offest;
            NetDelay = timeSum / timeQueue.Count;
        }
        private void SendHeartBeat()
        {
            var msg = new C2SHeartBeatMsg();
            msg.Index = sendIndex++;
            msg.SendTime = Utility.Time.MillisecondNow();
            GetModule<NetWorkMgr>().SendMsg(NetCmd.C2SHeartBeat, msg);
        }
    }
}
