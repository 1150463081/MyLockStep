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
        public HeroEntity MainHero { get; set; } 

        private Dictionary<int, EntityBase> entityDict = new Dictionary<int, EntityBase>();
        private Dictionary<uint, ISyncUnit> syncUnitDict = new Dictionary<uint, ISyncUnit>();
        private Dictionary<EntityType, List<EntityBase>> entityTypeDict = new Dictionary<EntityType, List<EntityBase>>();
        private HashSet<int> allUrl = new HashSet<int>();
        private int genUrlIndex = 100;

        public override void OnInit()
        {
            var enumArr = Enum.GetValues(typeof(EntityType));
            foreach (var item in enumArr)
            {
                entityTypeDict[(EntityType)item] = new List<EntityBase>();
            }
        }
        public HeroEntity AddHero(bool isMain, uint netUrl)
        {
            var heroEntity = GetModule<GameUnitMgr>().AccrueEntity<HeroEntity>();
            heroEntity.InitHero(isMain, netUrl);
            syncUnitDict[netUrl] = heroEntity;
            entityTypeDict[heroEntity.EntityType].Add(heroEntity);
            return heroEntity;
        }
        public T AccrueEntity<T>()
            where T : EntityBase, new()
        {
            T entity = new T();
            entity.Url = GenerateUrl();
            entityDict[entity.Url] = entity;
            return entity;
        }
        public bool HasSyncUnit(uint url)
        {
            return syncUnitDict.ContainsKey(url);
        }
        public List<EntityBase> GetEntitiyByType(EntityType type)
        {
            return entityTypeDict[type];
        }
        public ISyncUnit GetSyncUnit(uint id)
        {
            return syncUnitDict[id];
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
