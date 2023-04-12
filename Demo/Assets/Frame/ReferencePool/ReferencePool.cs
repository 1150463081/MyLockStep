using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepFrame
{
    public static partial class ReferencePool
    {
        private static Dictionary<Type, ReferenceCollection> collectionsDict = new Dictionary<Type, ReferenceCollection>(); 
        public static T Accrue<T>()
            where T : IReference,new()
        {
            var type = typeof(T);
            if (!collectionsDict.ContainsKey(type))
            {
                collectionsDict[type] = new ReferenceCollection(type);
            }
            return collectionsDict[type].Accrue<T>();
        }
        public static void Release(IReference reference)
        {
            if (reference == null)
            {
                return;
            }
            var type = reference.GetType();
            if (!collectionsDict.ContainsKey(type))
            {
                collectionsDict[type] = new ReferenceCollection(type);
            }
            collectionsDict[type].Release(reference);
        }
    }
}
