using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    [Module]
    public class TimerMgr : Module
    {
        MsTickTimer msTickTimer = new MsTickTimer();

        public override void Init()
        {

        }
        public override void Update()
        {
            msTickTimer.UpdateTask();
        }
        public int AddMsTickTimerTask(int delay, Action callEvt, Action cancelEvt, int loopCnt = 1)
        {
            return msTickTimer.AddTask(delay, callEvt, cancelEvt, loopCnt);
        }
    }
}
