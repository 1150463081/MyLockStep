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
        S2COpKey,
        C2SHeartBeat,
        S2CHeartBeat
    }
    [Serializable]
    public class NetMsg : KCPMsg,IProto
    {
        public uint SessionId;
        public NetCmd NetCmd;
        public long Time;

        public virtual void Clear()
        {
            SessionId = 0;
            NetCmd = NetCmd.None;
            Time = 0;
        }
    }
    [Serializable]
    public class C2SHeartBeatMsg : NetMsg
    {
        public int Index;
        public long SendTime;
        public override void Clear()
        {
            base.Clear();
            Index = 0;
            SendTime = 0;
        }
    }
    [Serializable]
    public class S2CHeartBeatMsg : NetMsg
    {
        public int Index;
        public long SendTime;
        public override void Clear()
        {
            base.Clear();
            Index = 0;
            SendTime = 0;
        }
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
        public override void Clear()
        {
            base.Clear();
            FrameStartTime = 0;
            PlayerId = null;
        }
    }
    [Serializable]
    public class C2SEnterBattleRoomMsg : NetMsg
    {
        public int RoomId;
        public override void Clear()
        {
            base.Clear();
            RoomId = 0;
        }
    }
    [Serializable]
    public class S2COpKeyMsg : NetMsg
    {
        public int FrameId;
        public List<OpKey> OpKeyList;
        public override void Clear()
        {
            base.Clear();
            FrameId = 0;
            OpKeyList = null;
        }
    }
    [Serializable]
    public class C2SOpKeyMsg : NetMsg
    {
        public int FrameId;
        public OpKey OpKey;
        public override void Clear()
        {
            base.Clear();
            FrameId = 0;
            ProtoPool.Release(OpKey);
            OpKey = null;
        }
    }
    [Serializable]
    public class OpKey:IProto
    {
        public uint PlayerId;
        public OpKeyType KeyType;
        public MoveKey MoveKey;

        public void Clear()
        {
            PlayerId = 0;
            KeyType = OpKeyType.None;
            ProtoPool.Release(MoveKey);
            MoveKey = null;
        }
    }
    public enum OpKeyType
    {
        None,
        Move
    }
    [Serializable]
    public class MoveKey:IProto
    {
        public long X_Value;
        public long Z_Value;

        public void Clear()
        {
            X_Value = 0;
            Z_Value = 0;
        }
    }
}
