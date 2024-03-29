﻿using System;
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
            public static Action<int> ClientLogicTickOver;
            public static Action<int> ClientFrameChange;
            public static Action<int> ServerFrameChange;
            public static Action<int> RealFrameChange;
        }
        public class Player
        {
            public static Action OnMainHeroAdd;
        }
    }
}
