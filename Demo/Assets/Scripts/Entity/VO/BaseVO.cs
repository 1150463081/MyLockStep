using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;

namespace GameCore
{
    public class BaseVO
    {
        public PEInt MoveSpeed { get; protected set; }
        public BaseVO()
        {
            MoveSpeed = 5;
        }
    }
}
