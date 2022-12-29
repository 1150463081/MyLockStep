using System;
using KCPNet;
using KcpProtocol;

namespace ServerExample
{
    class ServerStart
    {
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port = 0415;
            KCPNet<ServerSession, NetMsg> server = new KCPNet<ServerSession, NetMsg>();
            server.StartAsServer(ip, port);

            while (true)
            {
                var input = Console.ReadLine();
                if (input == "quit")
                {
                    server.CloseServer();
                    break;
                }
                else
                {
                    server.BroadCastMsg(new NetInfoMsg() { Cmd = CMD.Info, info = input });
                }
            }
            Console.ReadKey();
        }
    }
}
