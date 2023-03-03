using System;
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
            bool isMain = ModuleManager.Instance.GetModule<NetWorkMgr>().SessionId == mMsg.PlayerId;
            var battleMgr = ModuleManager.Instance.GetModule<BattleMgr>();
            ModuleManager.Instance.GetModule<BattleMgr>().AddHero(isMain, mMsg.PlayerId);
        }
    }
    public class S2COpKeyHandle : NetHandle
    {
        public override NetCmd NetCmd => NetCmd.S2COpKey;

        public override void Handle(NetMsg msg)
        {
            var mMsg = msg as S2COpKeyMsg;
            var battleMgr= ModuleManager.Instance.GetModule<BattleMgr>();
            battleMgr.InputKey(mMsg);
            battleMgr.LogicTick();
        }
    }
}
