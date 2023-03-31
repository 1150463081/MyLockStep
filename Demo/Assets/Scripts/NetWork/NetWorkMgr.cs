using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using LockStepFrame;
using KCPNet;
using NetProtocol;

namespace GameCore
{
    [Module]
    public class NetWorkMgr : Module
    {
        public uint SessionId => client.clientSession == null ? 0 : client.clientSession.SessionId;
        public const string ip = "10.0.10.34";
        public const int port = 0415;

        private KCPNet<ClientSession, NetMsg> client;
        private Dictionary<NetCmd, NetHandle> handleDict;
        private Queue<NetMsg> msgQueue = new Queue<NetMsg>();
        private static readonly string msgQueueLock = "lock_msgQueue";

        public override void OnInit()
        {
            client = new KCPNet<ClientSession, NetMsg>();
            InitHandle();
        }
        public override void OnStart()
        {
            StartConnect();
        }
        public override void OnUpdate()
        {
            HandleMsg();
        }
        public override void OnDestroy()
        {
            client.CloseClient();
        }
        public void StartConnect()
        {
            client.StartAsClient(ip, port);
            client.ConnectServer(200, 5000);
        }

        public void SendMsg(NetCmd cmd, NetMsg msg)
        {
            if (client.clientSession == null)
            {
                Debug.LogError("Not connected server");
                return;
            }
            msg.SessionId = client.clientSession.SessionId;
            msg.NetCmd = cmd;
            client.clientSession.SendMsg(msg);
        }
        public void EnqueueMsg(NetMsg msg)
        {
            lock (msgQueueLock)
            {
                msgQueue.Enqueue(msg);
            }
        }
        private void HandleMsg()
        {
            NetMsg msg;
            while (msgQueue.Count > 0)
            {
                msg = msgQueue.Dequeue();
                if (handleDict.ContainsKey(msg.NetCmd))
                {
                    handleDict[msg.NetCmd]?.Handle(msg);
                }
                else
                {
                    Debug.LogError($"没有{msg.NetCmd}对应的Handle");
                }
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
