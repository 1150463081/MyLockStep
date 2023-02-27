using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LockStepFrame;
using NetProtocol;

namespace GameCore
{
    public class EnterRoomPanel : UIBase
    {
        private Button btn_enterRoom;
        protected override void OnInit()
        {
            btn_enterRoom = transform.Find("btnEnterRoom").GetComponent<Button>();
            btn_enterRoom.onClick.AddListener(EnterRoomOnClick);
        }

        private void EnterRoomOnClick()
        {
            NetWorkMgr.SendMsg(NetCmd.C2SEnterBattleRoom, new C2SEnterBattleRoomMsg() { RoomId=1});
        }
    }
}
