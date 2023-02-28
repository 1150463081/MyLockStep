using System;
using KCPNet;

namespace NetProtocol
{
    public enum NetCmd
    {
        None,
        C2SEnterBattleRoom,
        S2CEnterBattleRoom,
        C2SOpKey,
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
    [Serializable]
    public class S2COpKeyMsg : NetMsg
    {
        public int FrameId;
        public MoveKey MoveKey; 
    }
    [Serializable]
    public class C2SOpKeyMsg : NetMsg
    {
        public MoveKey MoveKey;
    }
    [Serializable]
    public class MoveKey
    {
        public long X_Value;
        public long Z_Value;
    }
}
