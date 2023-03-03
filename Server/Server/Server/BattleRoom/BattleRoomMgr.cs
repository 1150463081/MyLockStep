using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class BattleRoomMgr : Singleton<BattleRoomMgr>
    {
        //房间号--房间
        private Dictionary<int, BattleRoom> roomDict = new Dictionary<int, BattleRoom>();
        //角色id--房间
        private Dictionary<uint, BattleRoom> playerIdRoomDict = new Dictionary<uint, BattleRoom>();

        public BattleRoom GetRoom(int roomId)
        {
            if (!roomDict.ContainsKey(roomId))
            {
                InnerCreateRoom(roomId);
            }
            return roomDict[roomId];
        }
        public BattleRoom GetRoomByPlayerId(uint playerId)
        {
            return playerIdRoomDict[playerId];
        }
        public void RegisterPlayer(uint playerId, BattleRoom room)
        {
            playerIdRoomDict[playerId] = room;
        }
        private void InnerCreateRoom(int roomId)
        {
            var room = new BattleRoom();
            room.Init(roomId);
            roomDict[roomId] = room;
        }

    }
}
