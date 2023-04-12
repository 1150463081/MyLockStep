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

            var netWorkMgr = ModuleManager.Instance.GetModule<NetWorkMgr>();
            var logictickMgr = ModuleManager.Instance.GetModule<LogicTickMgr>();
            var gameUnitMgr = ModuleManager.Instance.GetModule<GameUnitMgr>();
            logictickMgr.StartClientFrame(mMsg.FrameStartTime);
            for (int i = 0; i < mMsg.PlayerId.Count; i++)
            {
                if (gameUnitMgr.HasSyncUnit(mMsg.PlayerId[i]))
                {
                    continue;
                }
                gameUnitMgr.AddHero(netWorkMgr.SessionId == mMsg.PlayerId[i], mMsg.PlayerId[i]);

            }
        }
    }
    public class S2COpKeyHandle : NetHandle
    {
        public override NetCmd NetCmd => NetCmd.S2COpKey;

        public override void Handle(NetMsg msg)
        {
            var mMsg = msg as S2COpKeyMsg;
            var logicTickMgr = ModuleManager.Instance.GetModule<LogicTickMgr>();
            logicTickMgr.CheckOutOpKey(mMsg);
        }
    }
}
