using System;
using System.Collections.Generic;
using System.Text;
using NetProtocol;

namespace Server
{
    public class BattleRoom
    {
        public int RoomId { get; private set; }

        private List<uint> allPlayerId = new List<uint>();
        private Dictionary<uint, BattlePlayer> playerDict = new Dictionary<uint, BattlePlayer>();
        private List<C2SOpKeyMsg> opKeyList = new List<C2SOpKeyMsg>();
        private int frameIdx;
        public void Init(int roomId)
        {
            RoomId = roomId;
            //初始化帧计时
            ModuleManager.Instance.GetModule<TimerMgr>().AddMsTickTimerTask(ServerDefine.ServerLogicFrameIntervalMs, TickLogicFrame, null, 0);
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

            ModuleManager.Instance.GetModule<ServerMgr>().SendMsg(allPlayerId, NetCmd.S2CEnterBattleRoom, new S2CEnterBattleRoomMsg() { PlayerId = allPlayerId });
        }
        public void InputOpKey(C2SOpKeyMsg opKey)
        {
            opKeyList.Add(opKey);
        }
        private void TickLogicFrame()
        {
            frameIdx++;
            S2COpKeyMsg msg = new S2COpKeyMsg()
            {
                FrameId = frameIdx,
                OpKeyList = new List<OpKey>()
            };

            if (opKeyList.Count > 0)
            {
                for (int i = 0; i < opKeyList.Count; i++)
                {
                    var opkey = new OpKey()
                    {
                        PlayerId = opKeyList[i].SessionId,
                        MoveKey = opKeyList[i].MoveKey
                    };
                    msg.OpKeyList.Add(opkey);
                }
            }
            opKeyList.Clear();
            ModuleManager.Instance.GetModule<ServerMgr>().SendMsg(allPlayerId, NetCmd.S2COpKey, msg);
        }
    }
}
