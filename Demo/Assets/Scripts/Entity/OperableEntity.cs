﻿using NetProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;
using UnityEngine;

namespace GameCore
{
    //可操作单位
    public class OperableEntity : LogicEntity, ISyncUnit
    {
        public uint NetUrl { get; protected set; }
        //本地预测和输入的未验证指令字典
        //帧数--指令
        public Dictionary<int, OperateInfo> localOpDict = new Dictionary<int, OperateInfo>();


        /// <summary>
        /// 预测指令
        /// </summary>
        public OperateInfo ForcastOpkey(int frameId)
        {
            localOpDict.TryGetValue(frameId - 1, out var LastOperate);
            var operate = ReferencePool.Accrue<OperateInfo>();
            operate.FrameId = frameId;
            if (LastOperate == null || LastOperate.KeyType == OpKeyType.None)
            {
                operate.KeyType = OpKeyType.None;
            }
            else if (LastOperate.KeyType == OpKeyType.Move)
            {
                operate.KeyType = OpKeyType.Move;
                operate.InputDir = LastOperate.InputDir;
            }

            InputKey(operate);

            return operate;
        }
        public void InputKey(OperateInfo operateInfo)
        {
            switch (operateInfo.KeyType)
            {
                case OpKeyType.None:
                    MoveDir = FXVector3.zero;
                    break;
                case OpKeyType.Move:
                    MoveDir = operateInfo.InputDir.normalized;
                    break;
            }
        }
        //缓存指令
        public void CacheOperate(int frameId, OperateInfo operateInfo)
        {
            localOpDict[frameId] = operateInfo;
        }
        /// <summary>
        /// 校验之前的逻辑帧
        /// </summary>
        /// <param name="opKey"></param>
        /// <returns>和之前的预测帧是否一致</returns>
        public bool CheckOutOpKey(int sFrameId, OpKey opKey)
        {
            var operateInfo = GetOperateInfo(sFrameId);
            bool isEqual = operateInfo.Equals(opKey);

            return isEqual;
        }

        public void ReleaseOperateInfo(int frameId)
        {
            if (!localOpDict.ContainsKey(frameId))
            {
                return;
            }
            ReferencePool.Release(localOpDict[frameId]);
            localOpDict.Remove(frameId);
        }
        public void ReplaceOperateInfo(int frameId, OpKey opKey)
        {
            ReleaseOperateInfo(frameId);
            var operateInfo = ReferencePool.Accrue<OperateInfo>();
            operateInfo.Init(frameId,opKey);
            CacheOperate(frameId, operateInfo);
        }
        public OperateInfo GetOperateInfo(int frameId)
        {
            if (!localOpDict.ContainsKey(frameId))
            {
                localOpDict[frameId] = ReferencePool.Accrue<OperateInfo>();
                localOpDict[frameId].KeyType = OpKeyType.None;
                localOpDict[frameId].FrameId =frameId;
            }
            return localOpDict[frameId];
        }
    }
}
