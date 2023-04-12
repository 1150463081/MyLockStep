using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetProtocol;
using LockStepFrame;

namespace GameCore
{
    public class OperateInfo : IReference
    {
        public OpKeyType KeyType { get; set; }
        //摇杆方向
        public FXVector3 InputDir { get; set; }

        public void Clear()
        {
            KeyType = OpKeyType.None;
            InputDir = default;
        }

    }
}
