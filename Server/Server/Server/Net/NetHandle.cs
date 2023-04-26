using System;
using System.Collections.Generic;
using System.Text;
using NetProtocol;

namespace Server
{
    public abstract class NetHandle
    {
        public abstract NetCmd NetCmd { get; }
        public abstract void Handle(uint sessionId, NetMsg msg);
    }
    public class EnterBattleRoomHandle : NetHandle
    {

        public override NetCmd NetCmd => NetCmd.C2SEnterBattleRoom;

        public override void Handle(uint sessionId, NetMsg msg)
        {
            var mMsg = msg as C2SEnterBattleRoomMsg;
            var room = BattleRoomMgr.Instance.GetRoom(mMsg.RoomId);
            room.AddPlayer(sessionId);
        }
    }
    public class OpKeyHandle : NetHandle
    {
        public override NetCmd NetCmd => NetCmd.C2SOpKey;

        public override void Handle(uint sessionId, NetMsg msg)
        {
            var mMsg = msg as C2SOpKeyMsg;
            var room = BattleRoomMgr.Instance.GetRoomByPlayerId(sessionId);
            room.InputOpKey(mMsg);
        }
    }
    public class HeartBeatHandle : NetHandle
    {
        public override NetCmd NetCmd => NetCmd.C2SHeartBeat;
        public override void Handle(uint sessionId, NetMsg msg)
        {
            var mMsg = msg as C2SHeartBeatMsg;
            var s2cMsg = ProtoPool.Accrue<S2CHeartBeatMsg>();
            s2cMsg.Index = mMsg.Index;
            s2cMsg.SendTime = mMsg.SendTime;
            ModuleManager.Instance.GetModule<ServerMgr>().SendMsg(mMsg.SessionId, NetCmd.S2CHeartBeat, s2cMsg);
        }
    }
}
