using System;
using KCPNet;

namespace KcpProtocol
{
    public enum CMD
    {
        Info,
        Ping,
    }
    [Serializable]
    public class NetMsg:KCPMsg
    {
        public  CMD Cmd;
    }
    [Serializable]
    public class NetInfoMsg : NetMsg
    {
        public string info;
    }
    [Serializable]
    public class NetPingMsg : NetMsg
    {
    }
}
