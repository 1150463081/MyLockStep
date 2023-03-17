﻿using KCPNet;
using NetProtocol;
using System;
using UnityEngine;
using LockStepFrame;

namespace GameCore
{
    public class ClientSession : KCPSession<NetMsg>
    {
        protected override void OnConnected()
        {
            Debug.Log($"Connect Success,SessionId:{SessionId}");
        }

        protected override void OnDisConnected()
        {
            Debug.Log($"Disconnect...");
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
            //！！！在子线程中接受，需到主线程处理
           
            if(msg is S2COpKeyMsg mMsg)
            {
                if (mMsg.FrameId < 7)
                {
                    Debug.LogWarning($"S:{mMsg.FrameId},{mMsg.Time}");
                }
            }

            var netMgr = ModuleManager.Instance.GetModule<NetWorkMgr>();
            netMgr.EnqueueMsg(msg);
        }

        protected override void OnUpdate(DateTime now)
        {
        }
    }
}
