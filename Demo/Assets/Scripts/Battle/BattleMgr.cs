using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;
using NetProtocol;
using UnityEngine;

namespace GameCore
{
    [Module]
    public class BattleMgr : Module
    {
        public int NetFrameId { get; private set; }
        public int LocalFrameId { get; private set; }

        private Dictionary<uint, ISyncUnit> syncUnitDict = new Dictionary<uint, ISyncUnit>();
        private List<HeroEntity> heroList = new List<HeroEntity>();
        private HashSet<uint> allPlayers = new HashSet<uint>();


        public override void OnUpdate()
        {
            base.OnUpdate();
            for (int i = 0; i < heroList.Count; i++)
            {
                heroList[i].ViewTick();
            }
        }

        public bool HasPlayer(uint netUrl)
        {
            return allPlayers.Contains(netUrl);
        }
        public HeroEntity AddHero(bool isMain, uint netUrl)
        {
            var heroEntity = GetModule<GameUnitMgr>().AccrueEntity<HeroEntity>();
            heroEntity.InitHero(isMain, netUrl);
            syncUnitDict[netUrl] = heroEntity;
            heroList.Add(heroEntity);
            allPlayers.Add(netUrl);
            return heroEntity;
        }
        public void InputKey(S2COpKeyMsg msg)
        {
            NetFrameId = msg.FrameId;
            ISyncUnit unit;
            OpKey opKey;
            for (int i = 0; i < msg.OpKeyList.Count; i++)
            {
                opKey = msg.OpKeyList[i];
                if (syncUnitDict.TryGetValue(opKey.PlayerId, out unit))
                {
                    unit.InputKey(opKey);
                }
                else
                {
                    Debug.LogError($"不存在单位{opKey.PlayerId}");
                }
            }

            LogicTick();
            GetModule<RollBackMgr>().TakeSnapShot();
        }
        public void LogicTick()
        {
            //Tick Hero
            for (int i = 0; i < heroList.Count; i++)
            {
                heroList[i].LogicTick();
            }
        }
    }
}
