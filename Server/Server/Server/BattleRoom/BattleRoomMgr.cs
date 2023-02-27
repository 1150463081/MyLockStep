using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class BattleRoomMgr:Singleton<BattleRoomMgr>
    {
        private Dictionary<int, BattleRoom> roomDict=new Dictionary<int, BattleRoom>();

        public BattleRoom GetRoom(int roomId)
        {
            if (!roomDict.ContainsKey(roomId))
            {
                InnerCreateRoom(roomId);
            }
            return roomDict[roomId];
        }

        private void InnerCreateRoom(int roomId)
        {
            var room = new BattleRoom();
            room.Init(roomId);
            roomDict[roomId] = room;
        }
    }
}
