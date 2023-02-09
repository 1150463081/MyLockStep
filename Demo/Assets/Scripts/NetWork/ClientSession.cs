using KCPNet;
using NetProtocol;
using System;

namespace GameCore
{
    public class ClientSession : KCPSession<NetMsg>
    {
        protected override void OnConnected()
        {
            throw new NotImplementedException();
        }

        protected override void OnDisConnected()
        {
            throw new NotImplementedException();
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
            throw new NotImplementedException();
        }

        protected override void OnUpdate(DateTime now)
        {
            throw new NotImplementedException();
        }
    }
}
