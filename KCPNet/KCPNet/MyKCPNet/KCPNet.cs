using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace KCPNet
{
    public class KCPNet<T,K>
        where T:KCPSession<K>,new()
        where K:KCPMsg,new()
    {
        private UdpClient udp;
        private IPEndPoint remoteEndPoint;
        private CancellationTokenSource cts;

        public KCPNet()
        {
            cts = new CancellationTokenSource();
        }

        #region Server
        private Dictionary<uint, T> sessionDict;
        public void StartAsServer(string ip,int port)
        {
            sessionDict = new Dictionary<uint, T>();
            udp = new UdpClient(new IPEndPoint(IPAddress.Parse(ip), port));
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                udp.Client.IOControl((IOControlCode)(-1744830452), new byte[] { 0, 0, 0, 0 }, null);
            }
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Utility.Debug.ColorLog(KCPLogColor.Green, "Server Start...");
            Task.Run(ServerRecive,cts.Token);
        }
        public void CloseServer()
        {
            foreach (var item in sessionDict.Values)
            {
                item.CloseSession();
            }
            sessionDict = null;
            if (udp != null)
            {
                udp.Close();
                udp = null;
                cts.Cancel();
            }
        }
        private async void ServerRecive()
        {
            UdpReceiveResult result;
            while (true)
            {
                if (cts.IsCancellationRequested)
                {
                    Utility.Debug.ColorLog(KCPLogColor.Cyan, "SeverRecive Task is Cancelled.");
                    break;
                }
                result = await udp?.ReceiveAsync();
                uint sid = BitConverter.ToUInt32(result.Buffer,0);
                if (sid == 0)
                {
                    sid = GenerateSid();
                    var sidBytes= BitConverter.GetBytes(sid);
                    var bytes = new byte[8];
                    Array.Copy(sidBytes, 0, bytes, 4, 4);
                    SendUDPMsg(bytes, result.RemoteEndPoint);
                }
                else
                {
                    if(!sessionDict.TryGetValue(sid,out var session))
                    {
                        session = new T();
                        session.InitSession(sid, SendUDPMsg, result.RemoteEndPoint);
                        session.OnSessionClose = OnServerSessionClose;
                        lock (sessionDict)
                        {
                            sessionDict[sid] = session;
                        }
                    }
                    session.ReciveData(result.Buffer);
                }
            }
        }
        private void OnServerSessionClose(uint sid)
        {
            if (sessionDict.ContainsKey(sid))
            {
                lock (sessionDict)
                {
                    sessionDict.Remove(sid);
                    Utility.Debug.Warn($"Session:{sid} remove from sessionDic");
                }
            }
        }
        private uint sid = 1000;
        private uint GenerateSid()
        {
            lock (sessionDict)
            {
                sid++;
            }
            return sid;
        }
        #endregion

        #region Client
        public T clientSession;
        public void StartAsClient(string ip,int port)
        {
            Utility.Debug.ColorLog(KCPLogColor.Green, "Client Start...");
            udp = new UdpClient(0);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                udp.Client.IOControl((IOControlCode)(-1744830452), new byte[] { 0, 0, 0, 0 }, null);
            }
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Task.Run(ClientRecv, cts.Token);
        }
        async void ClientRecv()
        {
            UdpReceiveResult result;
            while (true)
            {
                if (cts.IsCancellationRequested)
                {
                    Utility.Debug.Warn("Task is Cancled...");
                }

                result = await udp.ReceiveAsync();
                if (Equals(remoteEndPoint,result.RemoteEndPoint))
                {
                    uint sid= BitConverter.ToUInt32(result.Buffer, 0);
                    if(sid == 0)//第一次收到消息，接受sid
                    {
                        if (clientSession != null && clientSession.IsConnected)
                        {
                            Utility.Debug.Warn("Client Has Connect,Discard Extra Sid");
                        }
                        else
                        {
                            sid = BitConverter.ToUInt32(result.Buffer, 4);
                            Utility.Debug.Log($"UDP Request Conv Sid:{sid}");

                            clientSession = new T();
                            clientSession.InitSession(sid,SendUDPMsg,remoteEndPoint);
                            clientSession.OnSessionClose = OnClientSessionClose;
                        }
                    }
                    else//处理业务逻辑
                    {
                        if (clientSession != null && clientSession.IsConnected)
                        {
                            clientSession.ReciveData(result.Buffer);
                        }
                        else
                        {
                            Utility.Debug.Warn("Client is Init...");
                        }
                    }
                }
            }
        }
        public void CloseClient()
        {
            if (clientSession != null)
            {
                clientSession.CloseSession();
            }
        }
        public Task<bool> ConnectServer(int interval,int maxCheckInterval=5000)
        {
            int checkTime = 0;
            SendUDPMsg(new byte[4], remoteEndPoint);
            var task= Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);
                    checkTime += interval;
                    if (clientSession != null && clientSession.IsConnected)
                    {
                        return true;
                    }
                    else {
                        if (checkTime > maxCheckInterval)
                        {
                            return false;
                        }
                    }
                }
            });
            return task;
        }
        private void OnClientSessionClose(uint sessionId)
        {
            cts.Cancel();
            if (udp != null)
            {
                udp.Close();
                udp = null;
            }
        }
        #endregion

        void SendUDPMsg(byte[] bytes ,IPEndPoint remotePoint)
        {
            if (udp != null)
            {
                udp.SendAsync(bytes, bytes.Length, remotePoint);
            }
        }
        public void BroadCastMsg(K msg)
        {
            var bytes = Utility.Bytes.Serialize(msg);
            foreach (var item in sessionDict.Values)
            {
                item.SendMsg(bytes);
            }
        }
    }
}
