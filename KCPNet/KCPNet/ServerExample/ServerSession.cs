using System;
using System.Collections.Generic;
using System.Text;
using KCPNet;
using KcpProtocol;

namespace ServerExample
{
    public class ServerSession : KCPSession<NetMsg>
    {
        protected override void OnConnected()
        {
            Utility.Debug.Log($"Client Connected:{SessionId}");
            checkTime = DateTime.UtcNow.AddSeconds(checkInterval);
        }

        protected override void OnDisConnected()
        {
            Utility.Debug.Log($"Client DisConnected:{SessionId}");
        }

        protected override void OnReciveMsg(NetMsg msg)
        {
            if (msg.Cmd == CMD.Info && msg is NetInfoMsg infoMsg)
            {
                Utility.Debug.Log($"Recive Client [{SessionId}] Msg:{infoMsg.info}");
            }
            else if (msg.Cmd == CMD.Ping)
            {
                Utility.Debug.Log($"Recive Ping,SessionId:{SessionId}");
                checkCnt = 0;
            }
        }

        protected override void OnUpdate(DateTime now)
        {
            if (now > checkTime)
            {
                checkTime = now.AddSeconds(checkInterval);
                checkCnt++;
                if (checkCnt > 3)
                {
                    CloseSession();
                }
            }
        }

        private float checkInterval = 5f;
        private int checkCnt;
        private DateTime checkTime;
    }
}
