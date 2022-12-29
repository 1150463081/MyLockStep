using System;
using KCPNet;
using KcpProtocol;
using System.Threading;
using System.Threading.Tasks;

namespace ClientExample
{
    class ClientStart
    {
        private static KCPNet<ClientSession, NetMsg> client;
        private static Task<bool> connectResult;
        private static int connectCnt;
        static void Main(string[] args)
        {
            string ip = "127.0.0.1";
            int port = 0415;
            client = new KCPNet<ClientSession, NetMsg>();
            client.StartAsClient(ip, port);
            connectResult = client.ConnectServer(200, 5000);
            Task.Run(CheckConnect);

            while (true)
            {
                var input = Console.ReadLine();
                if (input == "quit")
                {
                    client.CloseClient();
                }
                else
                {
                    client.clientSession.SendMsg(new NetInfoMsg()
                    {
                        Cmd= CMD.Info,
                        info = input
                    });
                }
            }
        }

        private static async void CheckConnect()
        {
            while (true)
            {
                await Task.Delay(3000);
                if (connectResult != null && connectResult.IsCompleted)
                {
                    if (connectResult.Result)
                    {
                        Utility.Debug.ColorLog(KCPLogColor.Green, "Connect Server Success!!!");
                        connectResult = null;
                        await Task.Run(SendPingMsg);
                    }
                    else
                    {
                        connectCnt++;
                        if (connectCnt > 4)
                        {
                            Utility.Debug.Error($"Connect Failed {connectCnt} Times!!!!");
                            connectResult = null;
                            break;
                        }
                        else
                        {
                            Utility.Debug.Warn($"Connect Failed {connectCnt} Times,Retry.....");
                            connectResult = client.ConnectServer(200, 5000);
                        }
                    }
                }
            }
        }
        private static async void SendPingMsg()
        {
            while (true)
            {
                await Task.Delay(5000);
                if (client != null && client.clientSession != null)
                {
                    client.clientSession.SendMsg(new NetPingMsg() {Cmd= CMD.Ping });
                    Utility.Debug.ColorLog(KCPLogColor.Green, "Client Send Ping Message.");
                }
                else
                {
                    Utility.Debug.ColorLog(KCPLogColor.Green, "Client Stop Ping!!!");
                    break;
                }
            }
        }
    }
}
