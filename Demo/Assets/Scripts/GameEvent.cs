using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public class GameEvent
    {
        public class LockStep
        {
            public static Action<int> LocalFrameChange;
            public static Action<int> NetFrameChange;
        }
    }
}
