using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LockStepFrame
{
    [Module]
    public class TimerManager : Module
    {
        MsTickTimer msTickTimer;


        public override void OnInit()
        {
            base.OnInit();
            msTickTimer = new MsTickTimer();
        }
        public override void OnUpdate()
        {
            msTickTimer.UpdateTask();
        }
        public override void OnFixedUpdate()
        {
        }
        public int AddMsTickTimerTask(int delay, Action callEvt, Action cancelEvt, int loopCnt = 1, long startTime = -1)
        {
            return msTickTimer.AddTask(delay, callEvt, cancelEvt, loopCnt, startTime);
        }

    }
}
