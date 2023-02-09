using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepFrame
{
    public class Timer
    {
        public int TimerId{ get; private set; }

        private float delayTime;
        private float waitTime;
        private float loopCount;
    }
}
