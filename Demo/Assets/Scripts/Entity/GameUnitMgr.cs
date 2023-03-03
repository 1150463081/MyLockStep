using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;

namespace GameCore
{
    [Module]
    public class GameUnitMgr : Module
    {
        private Dictionary<int, EntityBase> entityDict = new Dictionary<int, EntityBase>();
        private HashSet<int> allUrl = new HashSet<int>();
        private int genUrlIndex = 100;

        public T AccrueEntity<T>()
            where T : EntityBase, new()
        {
            T entity = new T();
            entity.Url = GenerateUrl();
            entityDict[entity.Url] = entity;
            return entity;
        }

        private int GenerateUrl()
        {
            while (allUrl.Contains(genUrlIndex))
            {
                genUrlIndex++;
            }
            return genUrlIndex;
        }
    }
}
