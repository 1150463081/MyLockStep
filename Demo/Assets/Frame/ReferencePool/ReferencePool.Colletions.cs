using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepFrame
{
    public static partial class ReferencePool
    {
        private class ReferenceCollection
        {
            private Type mType;

            private Queue<IReference> queue = new Queue<IReference>();
            public ReferenceCollection(Type type)
            {
                mType = type;
            }
            public T Accrue<T>()
                where T : IReference, new()
            {
                if (typeof(T) != mType)
                {
                    throw new Exception($"{typeof(T).ToString()} is invaild");
                }
                if (queue.Count > 0)
                {
                    return (T)queue.Dequeue();
                }
                return new T();
            }
            public void Release(IReference reference)
            {
                reference.Clear();
                queue.Enqueue(reference);
            }
        }
    }
}
