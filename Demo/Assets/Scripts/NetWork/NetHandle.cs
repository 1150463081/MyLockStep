﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;
using NetProtocol;

namespace GameCore
{
    public abstract class NetHandle
    {
        public abstract NetCmd NetCmd { get; }
        public abstract void Handle(NetMsg msg);
    }
    public class S2CEnterBattleRoomHandle : NetHandle
    {
        public override NetCmd NetCmd => NetCmd.S2CEnterBattleRoom;

        public override void Handle(NetMsg msg)
        {
            var mMsg = msg as S2CEnterBattleRoomMsg;

            var netWorkMgr = ModuleManager.Instance.GetModule<NetWorkMgr>();
            var logictickMgr = ModuleManager.Instance.GetModule<LogicTickMgr>();
            var gameUnitMgr = ModuleManager.Instance.GetModule<GameUnitMgr>();
            //创建角色
            for (int i = 0; i < mMsg.PlayerId.Count; i++)
            {
                if (gameUnitMgr.HasSyncUnit(mMsg.PlayerId[i]))
                {
                    continue;
                }
                gameUnitMgr.AddHero(netWorkMgr.SessionId == mMsg.PlayerId[i], mMsg.PlayerId[i]);

            }
            logictickMgr.StartClientFrame(mMsg);
        }
    }
    public class S2COpKeyHandle : NetHandle
    {
        public override NetCmd NetCmd => NetCmd.S2COpKey;

        public override void Handle(NetMsg msg)
        {
            var mMsg = msg as S2COpKeyMsg;
            var logicTickMgr = ModuleManager.Instance.GetModule<LogicTickMgr>();
            logicTickMgr.InputOpKey(mMsg);
        }
    }
    public class S2CHeartBeatHandle : NetHandle
    {
        public override NetCmd NetCmd => NetCmd.S2CHeartBeat;
        public override void Handle(NetMsg msg)
        {
            var mMsg = msg as S2CHeartBeatMsg;
            ModuleManager.Instance.GetModule<HeartBeatMgr>().ReciveHeartBeat(mMsg);
        }
    }
}
