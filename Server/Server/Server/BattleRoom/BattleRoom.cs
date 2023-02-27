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
        public void Init(int roomId)
        {
            RoomId = roomId;
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

            ServerMgr.Instance.SendMsg(allPlayerId, NetCmd.S2CEnterBattleRoom, new S2CEnterBattleRoomMsg() { PlayerId = sessionId });
        }
    }
}
