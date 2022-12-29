using System;
using System.Collections.Generic;
using System.Text;
using KCPNet;
using KcpProtocol;

namespace ClientExample
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
            if(msg is NetInfoMsg infoMsg)
            {
                Utility.Debug.Log($"ReciveInfo:{infoMsg.info}");
            }
        }

        protected override void OnUpdate(DateTime now)
        {
        }
    }
}
