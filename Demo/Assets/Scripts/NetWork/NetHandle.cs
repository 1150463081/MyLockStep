using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetProtocol;

namespace GameCore
{
    public abstract class NetHandle
    {
        public abstract NetCmd NetCmd { get; }
        public abstract void Handle(NetMsg msg);
    }
    public class S2CEnterBattleRoomHandle : NetHandle
    {
        public override NetCmd NetCmd => NetCmd.S2CEnterBattleRoom;

        public override void Handle(NetMsg msg)
        {
            var mMsg = msg as S2CEnterBattleRoomMsg;
        }
    }
}
