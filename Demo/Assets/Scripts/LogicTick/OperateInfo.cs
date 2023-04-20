using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetProtocol;
using LockStepFrame;
using UnityEngine;

namespace GameCore
{
    public class OperateInfo : IReference
    {
        public int FrameId { get; set; }
        public OpKeyType KeyType { get; set; }
        //摇杆方向
        public FXVector3 InputDir { get; set; }

        public void Init(int frameId, OpKey opKey)
        {
            FrameId = frameId;
            KeyType = opKey.KeyType;
            if(KeyType== OpKeyType.Move)
            {
                FXInt x_Value = new FXInt(opKey.MoveKey.X_Value);
                FXInt z_value = new FXInt(opKey.MoveKey.Z_Value);
                InputDir = new FXVector3(x_Value, 0, z_value);
            }
        }
        public void Clear()
        {
            KeyType = OpKeyType.None;
            InputDir = default;
        }
        public bool Equals(OpKey opKey)
        {
            if (opKey.KeyType != KeyType)
            {
                return false;
            }
            else if (KeyType == OpKeyType.Move)
            {
                if(InputDir.x.ScaledValue!=opKey.MoveKey.X_Value||
                    InputDir.z.ScaledValue != opKey.MoveKey.Z_Value)
                {
                    Debug.LogError($"NotEquals,FrameId:{FrameId}");
                    return false;
                }
            }
            return true;
        }
    }
}
