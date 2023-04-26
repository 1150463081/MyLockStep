using System;
using System.Collections.Generic;
using System.Text;
using NetProtocol;
using System.Linq;

namespace Server
{
    public class BattleRoom
    {
        public int RoomId { get; private set; }

        private List<uint> allPlayerId = new List<uint>();
        private Dictionary<uint, BattlePlayer> playerDict = new Dictionary<uint, BattlePlayer>();
        private Dictionary<int, List<C2SOpKeyMsg>> opKeyDict = new Dictionary<int, List<C2SOpKeyMsg>>();
        private int frameIdx;
        private long frameStartTime;
        public void Init(int roomId)
        {
            RoomId = roomId;
            //初始化帧计时
            ModuleManager.Instance.GetModule<TimerMgr>().AddMsTickTimerTask(ServerDefine.ServerLogicFrameIntervalMs, TickLogicFrame, null, 0);
            frameStartTime = Utility.Time.MillisecondNow();
        }
        public void AddPlayer(uint sessionId)
        {
            if (allPlayerId.Contains(sessionId))
            {
                Utility.Log.Error($"{sessionId} Is In Room,Add Failed");
                return;
            }
            allPlayerId.Add(sessionId);
            var player = new BattlePlayer();
            player.Init();
            playerDict[sessionId] = player;

            BattleRoomMgr.Instance.RegisterPlayer(sessionId, this);
            var msg = ProtoPool.Accrue<S2CEnterBattleRoomMsg>();
            msg.FrameStartTime = frameStartTime;
            msg.PlayerId = allPlayerId;
            ModuleManager.Instance.GetModule<ServerMgr>().SendMsg(allPlayerId, NetCmd.S2CEnterBattleRoom, msg);
        }
        public void InputOpKey(C2SOpKeyMsg opKey)
        {
            if (!opKeyDict.ContainsKey(opKey.FrameId))
            {
                opKeyDict[opKey.FrameId] = new List<C2SOpKeyMsg>();
            }
            opKeyDict[opKey.FrameId].Add(opKey);
        }
        private void TickLogicFrame()
        {
            frameIdx++;

            var msg = ProtoPool.Accrue<S2COpKeyMsg>();
            msg.FrameId = frameIdx;
            msg.OpKeyList = new List<OpKey>();
            opKeyDict.TryGetValue(frameIdx, out var opKeyList);

            if (opKeyList!=null&&opKeyList.Count > 0)
            {
                for (int i = 0; i < opKeyList.Count; i++)
                {
                    var opKey = ProtoPool.Accrue<OpKey>();
                    opKey.PlayerId = opKeyList[i].SessionId;
                    opKey.KeyType = opKeyList[i].OpKey.KeyType;
                    opKey.MoveKey = ProtoPool.Accrue<MoveKey>();
                    opKey.MoveKey.X_Value = opKeyList[i].OpKey.MoveKey.X_Value;
                    opKey.MoveKey.Z_Value = opKeyList[i].OpKey.MoveKey.Z_Value;
                    msg.OpKeyList.Add(opKey);
                }
            }

            msg.Time = Utility.Time.MillisecondNow();
            ModuleManager.Instance.GetModule<ServerMgr>().SendMsg(allPlayerId, NetCmd.S2COpKey, msg);
        }
    }
}
