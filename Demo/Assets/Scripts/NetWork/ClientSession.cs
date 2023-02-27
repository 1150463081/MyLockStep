using KCPNet;
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
            Debug.Log($"Recive Msg:{msg.NetCmd}");
            var netMgr = ModuleManager.Instance.GetModule<NetWorkMgr>();
            netMgr.ReciveMsg(msg);
        }

        protected override void OnUpdate(DateTime now)
        {
        }
    }
}
