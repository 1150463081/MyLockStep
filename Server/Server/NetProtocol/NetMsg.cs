using System;
using KCPNet;
using System.Collections.Generic;

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
        public long Time;
    }
    [Serializable]
    public class S2CEnterBattleRoomMsg : NetMsg
    {
        //服务器逻辑帧开始时间戳
        public long FrameStartTime;
        /// <summary>
        /// 房间的玩家
        /// </summary>
        public List<uint> PlayerId;
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
        public List<OpKey> OpKeyList;
    }
    [Serializable]
    public class C2SOpKeyMsg : NetMsg
    {
        public OpKey OpKey;
    }
    [Serializable]
    public class OpKey 
    {
        public uint PlayerId;
        public OpKeyType KeyType;
        public MoveKey MoveKey;
    }
    public enum OpKeyType
    {
        None,
        Move
    }
    [Serializable]
    public class MoveKey
    {
        public long X_Value;
        public long Z_Value;
    }
}
