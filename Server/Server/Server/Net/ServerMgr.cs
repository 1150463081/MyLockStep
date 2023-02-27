﻿using KCPNet;
using NetProtocol;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server
{
    public class ServerMgr : Singleton<ServerMgr>
    {
        public const string ip = "127.0.0.1";
        public const int port = 0415;

        KCPNet<ServerSession, NetMsg> server;
        Dictionary<NetCmd, NetHandle> handleDict;
        public void StartServer()
        {
            InitHandle();
            server = new KCPNet<ServerSession, NetMsg>();
            server.StartAsServer(ip, port);
        }
        public void Tick()
        {

        }
        public void SendMsg(uint sessionId, NetMsg msg)
        {
            server.SendMsg(sessionId, msg);
        }
        public void SendMsg(IList<uint> sessionIds,NetMsg msg)
        {
            for (int i = 0; i < sessionIds.Count; i++)
            {
                SendMsg(sessionIds[i], msg);
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