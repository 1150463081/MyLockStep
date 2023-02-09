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
            Utility.Debug.Log($"Session {SessionId} OnConnected");
        }

        protected override void OnDisConnected()
        {
            Utility.Debug.Log($"Session {SessionId} OnDisConnected");
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
            //todo RecvMsg
        }

        protected override void OnUpdate(DateTime now)
        {
        }
    }
}
