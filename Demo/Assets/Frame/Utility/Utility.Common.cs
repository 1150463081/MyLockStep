using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LockStepFrame
{
    public partial class Utility
    {
        public class Common
        {
            private static Dictionary<Type, int> indexDic=new Dictionary<Type, int>();
            private static Dictionary<Type, Queue<int>> recyleDic=new Dictionary<Type, Queue<int>>();
            public static int GenerateId<T>()
            {
                Type type = typeof(T);
                if(recyleDic.TryGetValue(type,out var queue))
                {
                    if (queue.Count > 0)
                    {
                        return queue.Dequeue();
                    }
                }
                else
                {
                    recyleDic[type] = new Queue<int>();
                }
                if(!indexDic.ContainsKey(type))
                {
                    indexDic[type] = 0;
                }
                int index = indexDic[type];
                indexDic[type] = index + 1;
                return index;
            }
            public static void RecyleId<T>(int id)
            {
                Type type = typeof(T);
                if (recyleDic.TryGetValue(type,out var queue))
                {
                    queue.Enqueue(id);
                }
                else
                {
                    Debug.LogError($"未生成过{type.ToString()}类型的id，回收{id}失败");
                }
            }
        }
    }
}
