using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets.Kcp;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace KCPNet
{
    public enum SessionState
    {
        None,
        Connected,
        DisConnected
    }
    public abstract class KCPSession<T>
        where T : KCPMsg, new()
    {
        private Kcp m_kcp;
        private KCPHandle m_kcpHandle;
        private IPEndPoint m_remotePoint;
        private CancellationTokenSource cts;
        protected SessionState m_sessionState = SessionState.None;
        public Action<uint> OnSessionClose;
        public uint SessionId { get; private set; }
        public bool IsConnected => m_sessionState == SessionState.Connected;

        protected abstract void OnUpdate(DateTime now);
        protected abstract void OnReciveMsg(T msg);
        protected abstract void OnConnected();

        protected abstract void OnDisConnected();

        public void InitSession(uint sid, Action<byte[], IPEndPoint> udpSender, IPEndPoint remotePoint)
        {
            SessionId = sid;
            m_sessionState = SessionState.Connected;
            m_remotePoint = remotePoint;

            m_kcpHandle = new KCPHandle();
            m_kcp = new Kcp(sid, m_kcpHandle);
            m_kcp.NoDelay(1, 10, 2, 1);
            m_kcp.WndSize(64, 64);
            m_kcp.SetMtu(512);

            m_kcpHandle.OutEvent = (Memory<byte> buffer) =>
              {
                  byte[] bytes = buffer.ToArray();
                  udpSender?.Invoke(bytes, remotePoint);
              };

            //对经过kcp处理过的消息进行接收处理
            m_kcpHandle.RecvEvent = (byte[] buffer) =>
              {
                  buffer = Utility.Bytes.Decompress(buffer);
                  var msg = Utility.Bytes.Deserialize<T>(buffer);
                  if (msg != null)
                  {
                      OnReciveMsg(msg);
                  }
              };

            OnConnected();

            cts = new CancellationTokenSource();
            Task.Run(Update, cts.Token);
        }
        public void CloseSession()
        {
            //todo
            cts.Cancel();
            OnDisConnected();

            OnSessionClose?.Invoke(SessionId);
            OnSessionClose = null;

            m_sessionState = SessionState.DisConnected;
            m_remotePoint = null;
            SessionId = 0;

            m_kcpHandle = null;
            m_kcp = null;
        }
        public void ReciveData(byte[] buffer)
        {
            m_kcp.Input(buffer);
        }
        public void SendMsg(T msg)
        {
            if (IsConnected)
            {
                var bytes= Utility.Bytes.Serialize(msg);
                SendMsg(bytes);
            }
            else
            {
                Utility.Debug.Warn("Client Disconnected!Can Not Send Messege!");
            }
        }
        public void SendMsg(byte[] bytes)
        {
            if (IsConnected)
            {
                bytes = Utility.Bytes.Compress(bytes);
                m_kcp.Send(bytes.AsSpan());
            }
            else
            {
                Utility.Debug.Warn("Client Disconnected!Can Not Send Messege!");
            }
        }
        private async void Update()
        {
            try
            {
                while (true)
                {
                    DateTime now = DateTime.UtcNow;
                    OnUpdate(now);
                    if (cts.IsCancellationRequested)
                    {
                        Utility.Debug.Warn("Session Task is Canceled");
                        break;
                    }
                    m_kcp.Update(now);
                    int len;
                    while ((len = m_kcp.PeekSize()) > 0)
                    {
                        byte[] buffer = new byte[len];
                        if (m_kcp.Recv(buffer) > 0)
                        {
                            m_kcpHandle.Recive(buffer);
                        }
                    }
                    await Task.Delay(10);
                }
            }
            catch (Exception e)
            {
                Utility.Debug.Error($"Session Update Exception:{e.ToString()}");
            }
        }
    }
}
