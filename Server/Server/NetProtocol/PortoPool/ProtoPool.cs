using System;
using System.Collections.Generic;
using System.Text;

namespace NetProtocol
{
    public static class ProtoPool
    {
        public static Dictionary<Type, Queue<IProto>> protoDict = new Dictionary<Type, Queue<IProto>>();
        public static T Accrue<T>()
            where T : IProto, new()
        {
            var type = typeof(T);
            if (!protoDict.ContainsKey(type))
            {
                protoDict[type] = new Queue<IProto>();
            }
            var queue = protoDict[type];
            if (queue.Count > 0)
            {
                return (T)queue.Dequeue();
            }
            return new T();
        }
        public static void Release(IProto proto)
        {
            if (proto == null)
            {
                return;
            }
            var type = proto.GetType();
            if (!protoDict.ContainsKey(type))
            {
                protoDict[type] = new Queue<IProto>();
            }
            proto.Clear();
            protoDict[type].Enqueue(proto);
        }
        public static void Release(IList<IProto> protos)
        {
            for (int i = 0; i < protos.Count; i++)
            {
                Release(protos[i]);
            }
        }
    }
}
