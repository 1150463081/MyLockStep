using KCPNet;
using NetProtocol;
using System;

namespace GameCore
{
    public class ClientSession : KCPSession<NetMsg>
    {
        protected override void OnConnected()
        {
        }

        protected override void OnDisConnected()
        {
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
        }

        protected override void OnUpdate(DateTime now)
        {
        }
    }
}
