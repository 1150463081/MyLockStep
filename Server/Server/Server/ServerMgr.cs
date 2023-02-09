using System;
using System.Collections.Generic;
using System.Text;
using KCPNet;
using NetProtocol;

namespace Server
{
    public class ServerMgr:Singleton<ServerMgr>
    {
        public const string ip = "127.0.0.1";
        public const int port = 0415;

        KCPNet<ServerSession, NetMsg> server;
        public void StartServer()
        {
            server = new KCPNet<ServerSession, NetMsg>();
            server.StartAsServer(ip, port);
        }
        public void Tick()
        {

        }
    }
}
