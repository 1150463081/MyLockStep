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
        private Text txt_localFrame;
        private Text txt_NetFrame;
        private InputField inputField;
        private Button btn_rollBack;


        private int rollBackFrame;
        protected override void OnInit()
        {
            btn_enterRoom = transform.Find("btnEnterRoom").GetComponent<Button>();
            txt_localFrame = transform.Find("txtLocalFrame").GetComponent<Text>();
            txt_NetFrame = transform.Find("txtNetFrame").GetComponent<Text>();
            inputField = transform.Find("InputField").GetComponent<InputField>();
            btn_rollBack = transform.Find("btnRollBack").GetComponent<Button>();
            btn_enterRoom.onClick.AddListener(EnterRoomOnClick);
            inputField.onEndEdit.AddListener(OnEndEdit);
            btn_rollBack.onClick.AddListener(OnRollBackClick);

            GameEvent.LockStep.NetFrameChange += NetFrameChangeHandler;
        }

        private void OnEndEdit(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return;
            }
            if (int.TryParse(str, out var value))
            {
                rollBackFrame = value;
            }
            else
            {
                inputField.text = string.Empty;
            }
        }
        private void OnRollBackClick()
        {
            GetModule<RollBackMgr>().RollBackTo(rollBackFrame);
        }
        private void EnterRoomOnClick()
        {
            NetWorkMgr.SendMsg(NetCmd.C2SEnterBattleRoom, new C2SEnterBattleRoomMsg() { RoomId = 1 });
        }
        private void NetFrameChangeHandler(int frame)
        {
            txt_NetFrame.text = "NetFrame:" + frame;
        }
    }
}
