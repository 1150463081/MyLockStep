using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;
using KCPNet;
using NetProtocol;

namespace GameCore
{
    public class NetWorkMgr:Singleton<NetWorkMgr>
    {
        public const string ip = "127.0.0.1";
        public const int port = 0415;

        private KCPNet<ClientSession, NetMsg> client;   

        public void Init()
        {
            client = new KCPNet<ClientSession, NetMsg>();   
        }
        public void StartConnect()
        {
            client.StartAsClient(ip, port);
            client.ConnectServer(200, 5000);
        }
    }
}
