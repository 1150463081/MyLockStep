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

            ModuleManager.Instance.GetModule<ServerMgr>().SendMsg(allPlayerId, NetCmd.S2CEnterBattleRoom, new S2CEnterBattleRoomMsg() { FrameStartTime = frameStartTime, PlayerId = allPlayerId });
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
            S2COpKeyMsg msg = new S2COpKeyMsg()
            {
                FrameId = frameIdx,
                OpKeyList = new List<OpKey>()
            };
            opKeyDict.TryGetValue(frameIdx, out var opKeyList);

            if (opKeyList!=null&&opKeyList.Count > 0)
            {
                for (int i = 0; i < opKeyList.Count; i++)
                {
                    var opkey = new OpKey()
                    {
                        PlayerId = opKeyList[i].SessionId,
                        MoveKey = opKeyList[i].OpKey.MoveKey,
                        KeyType = opKeyList[i].OpKey.KeyType
                    };
                    msg.OpKeyList.Add(opkey);
                }
            }

            msg.Time = Utility.Time.MillisecondNow();
            ModuleManager.Instance.GetModule<ServerMgr>().SendMsg(allPlayerId, NetCmd.S2COpKey, msg);
        }
    }
}
