using KCPNet;
using NetProtocol;
using System;
using System.Collections.Generic; 
using System.Linq;

namespace Server
{
    [Module]
    public class ServerMgr :Module
    {
        public const string ip = "10.0.10.34";
        public const int port = 0415;

        KCPNet<ServerSession, NetMsg> server;
        Dictionary<NetCmd, NetHandle> handleDict;


        public override void Init()
        {
            StartServer();
        }
        public override void Update()
        {
        }
        public void StartServer()
        {
            InitHandle();
            server = new KCPNet<ServerSession, NetMsg>();
            server.StartAsServer(ip, port);
        }
        public void SendMsg(uint sessionId, NetCmd cmd, NetMsg msg)
        {
            msg.NetCmd = cmd;
            server.SendMsg(sessionId, msg);
            ProtoPool.Release(msg);
        }
        public void SendMsg(IList<uint> sessionIds, NetCmd cmd, NetMsg msg)
        {
            msg.NetCmd = cmd;
            for (int i = 0; i < sessionIds.Count; i++)
            {
                server.SendMsg(sessionIds[i], msg);
            }
        }
        public void ReceiveMsg(uint sessionId, NetMsg msg)
        {
            if (handleDict.TryGetValue(msg.NetCmd, out var handle))
            {
                handle.Handle(sessionId, msg);
            }
        }
        private void InitHandle()
        {
            handleDict = new Dictionary<NetCmd, NetHandle>();
            var type = typeof(NetHandle);
            var types = type.Assembly.GetTypes();
            types = types.Where(t => { return type.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && !t.Equals(type); }).ToArray();
            int len = types.Length;
            for (int i = 0; i < len; i++)
            {
                var handle = Activator.CreateInstance(types[i]) as NetHandle;
                handleDict[handle.NetCmd] = handle;
            }
        }
    }
}
