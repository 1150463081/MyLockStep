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
            //if (x != 0 || z != 0)
            //{
            //    InputMoveKey(x, z);
            //}
            //else if (x != lastX || z != lastZ)
            //{
            //    InputMoveKey(x, z);
            //}
            InputMoveKey(x, z);
            lastX = x;
            lastZ = z;

        }
        private void InputMoveKey(float x, float z)
        {
            FXInt fixedX = (FXInt)x;
            FXInt fixedZ = (FXInt)z;
            var operateInfo = ReferencePool.Accrue<OperateInfo>();
            operateInfo.FrameId = GetModule<LogicTickMgr>().CFrameId;
            operateInfo.KeyType = OpKeyType.Move;
            operateInfo.InputDir = new FXVector3(fixedX, 0, fixedZ);
            GetModule<LogicTickMgr>().SendOpKey(operateInfo);
        }
       
    }
}
