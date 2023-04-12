using NetProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;

namespace GameCore
{
    //可操作单位
    public class OperableEntity : LogicEntity
    {
        public OperateInfo NowOperate;
        public OperateInfo LastOperate;
       
        /// <summary>
        /// 预测指令
        /// </summary>
        public void ForcastOpkey()
        {
            var operate= ReferencePool.Accrue<OperateInfo>();
            if (LastOperate==null||LastOperate.KeyType== OpKeyType.None)
            {
                operate.KeyType = OpKeyType.None;
            }
            else if (LastOperate.KeyType == OpKeyType.Move)
            {
                operate.KeyType = OpKeyType.Move;
                operate.InputDir = LastOperate.InputDir;
            }
            LastOperate = NowOperate;
            NowOperate = operate;

            InputKey(NowOperate);
        }
        public void InputKey(OperateInfo operateInfo)
        {
            localOpQueue.Enqueue(operateInfo);
            switch (operateInfo.KeyType)
            {
                case OpKeyType.None:
                    MoveDir = FXVector3.zero;
                    break;
                case OpKeyType.Move:
                    MoveDir = NowOperate.InputDir;
                    break;
            }
        }
    }
}
