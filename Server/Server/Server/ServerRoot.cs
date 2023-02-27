using System;
using System.Threading;

namespace Server
{
    class ServerRoot
    {
        static void Main(string[] args)
        {
            ServerMgr.Instance.StartServer();
            while (true)
            {
                ServerMgr.Instance.Tick();
                Thread.Sleep(10);
            }   
        }
    }
}
