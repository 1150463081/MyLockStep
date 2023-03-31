using System;
using System.Collections.Generic;
using LockStepFrame;
using UnityEngine;
using NetProtocol;

namespace GameCore
{
    [Module]
    public class InputMgr : Module
    {
        float lastX;
        float lastZ;
        public override void OnUpdate()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            if (x != 0 || z != 0)
            {
                InputMoveKey(x, z);
            }
            else if (x != lastX || z != lastZ)
            {
                InputMoveKey(x, z);
            }
            lastX = x;
            lastZ = z;
        }
        public void InputMoveKey(float x, float z)
        {
            FXInt fixedX = (FXInt)x;
            FXInt fixedZ = (FXInt)z;
            MoveKey moveKey = new MoveKey();
            moveKey.X_Value = fixedX.ScaledValue;
            moveKey.Z_Value = fixedZ.ScaledValue;
            OpKey opKey = new OpKey() { MoveKey = moveKey, KeyType = OpKeyType.Move };
            GetModule<NetWorkMgr>().SendMsg(NetCmd.C2SOpKey, new C2SOpKeyMsg() { OpKey = opKey });
        }
    }
}
