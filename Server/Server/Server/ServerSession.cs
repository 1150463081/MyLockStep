using System;
using System.Collections.Generic;
using System.Text;
using KCPNet;
using NetProtocol;

namespace Server
{
    public class ServerSession : KCPSession<NetMsg>
    {
        protected override void OnConnected()
        {
            Utility.Log.Debug($"Session {SessionId} OnConnected");
        }

        protected override void OnDisConnected()
        {
            Utility.Log.Debug($"Session {SessionId} OnDisConnected");
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
            //todo RecvMsg
            ModuleManager.Instance.GetModule<ServerMgr>().ReceiveMsg(SessionId, msg);
        }

        protected override void OnUpdate(DateTime now)
        {
        }
    }
}
