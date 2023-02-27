using System;
using KCPNet;

namespace NetProtocol
{
    public enum NetCmd
    {
        None,
        C2SEnterBattleRoom,
        S2CEnterBattleRoom,
        S2COpKey

    }
    [Serializable]
    public class NetMsg : KCPMsg
    {
        public uint SessionId;
        public NetCmd NetCmd;
    }
    [Serializable]
    public class S2CEnterBattleRoomMsg : NetMsg
    {
        /// <summary>
        /// 新加入房间的玩家
        /// </summary>
        public uint PlayerId;
    }
    [Serializable]
    public class C2SEnterBattleRoomMsg : NetMsg
    {
        public int RoomId;
    }
    public class S2COpKey : NetMsg
    {
        public MoveKey MoveKey; 
    }
    public class MoveKey
    {
        public long X_Value;
        public long Y_Value;
    }
}
