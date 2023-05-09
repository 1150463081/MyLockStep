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
        private Text txt_RealFrame;
        private Text txt_Offest;
        private Text txt_NetUrl;
        private Text txt_Pos;
        private Text txt_Delay;
        private InputField input_RollBack;
        private Button btn_rollBack;
        private InputField input_ChangeAheadFrame;
        private Button btn_ChangeAheadFrame;

        private int sFrame;
        private int cFrame;
        private int rFrame;

        private int rollBackFrame;
        private int aheadFrame;
        protected override void OnInit()
        {
            btn_enterRoom = transform.Find("btnEnterRoom").GetComponent<Button>();
            txt_localFrame = transform.Find("txtLocalFrame").GetComponent<Text>();
            txt_NetFrame = transform.Find("txtNetFrame").GetComponent<Text>();
            txt_RealFrame = transform.Find("txtRealFrame").GetComponent<Text>();
            txt_Offest = transform.Find("txtOffest").GetComponent<Text>();
            input_RollBack = transform.Find("InputField").GetComponent<InputField>();
            btn_rollBack = transform.Find("btnRollBack").GetComponent<Button>();
            txt_NetUrl = transform.Find("txtNetUrl").GetComponent<Text>();
            txt_Pos = transform.Find("txtPos").GetComponent<Text>();
            txt_Delay = transform.Find("txtDelay").GetComponent<Text>();
            input_ChangeAheadFrame = transform.Find("input_ChangeAheadFrame").GetComponent<InputField>();
            btn_ChangeAheadFrame = transform.Find("btn_ChangeAheadFrame").GetComponent<Button>();
            btn_enterRoom.onClick.AddListener(EnterRoomOnClick);
            input_RollBack.onEndEdit.AddListener(OnEndEdit);
            btn_rollBack.onClick.AddListener(OnRollBackClick);
            input_ChangeAheadFrame.onEndEdit.AddListener(OnChangeAheadFrameEndEdit);
            btn_ChangeAheadFrame.onClick.AddListener(OnChangeAheadFrameClick);

            GameEvent.LockStep.ServerFrameChange += ServerFrameChangeHandler;
            GameEvent.LockStep.ClientFrameChange += ClientFrameChangeHandler;
            GameEvent.LockStep.RealFrameChange += RealFrameChangeHandler;
            GameEvent.Player.OnMainHeroAdd += OnMainHeroAddHandler;
        }
        protected override void OnStart()
        {
            GetModule<UpdateMgr>().UpdateEvent += OnUpdateHandler;
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
                input_RollBack.text = string.Empty;
            }
        }

        private void OnRollBackClick()
        {
            GetModule<RollBackMgr>().RollBackTo(rollBackFrame);
        }
        private void OnChangeAheadFrameEndEdit(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return;
            }
            if (int.TryParse(str, out var value))
            {
                aheadFrame = value;
            }
            else
            {
                input_ChangeAheadFrame.text = string.Empty;
            }
        }
        private void OnChangeAheadFrameClick()
        {
            GetModule<LogicTickMgr>().ChangeAheadFrame(aheadFrame);
        }
        private void EnterRoomOnClick()
        {
            NetWorkMgr.SendMsg(NetCmd.C2SEnterBattleRoom, new C2SEnterBattleRoomMsg() { RoomId = 1 });
        }
        private void ServerFrameChangeHandler(int frame)
        {
            txt_NetFrame.text = "ServerFrame:" + frame;
            sFrame = frame;
            txt_Offest.text = "Offest:" + (cFrame - sFrame) + ";" + (cFrame - rFrame);
        }
        private void ClientFrameChangeHandler(int frame)
        {
            txt_localFrame.text = "ClientFrame:" + frame;
            cFrame = frame;
            txt_Offest.text = "Offest:" + (cFrame - sFrame) + ";" + (cFrame - rFrame);
        }
        private void RealFrameChangeHandler(int frame)
        {
            txt_RealFrame.text = "RealFrame:" + frame;
            rFrame = frame;
            txt_Offest.text = "Offest:" + (cFrame - sFrame) + ";" + (cFrame - rFrame);
        }
        private void OnMainHeroAddHandler()
        {
            var mainHero = GetModule<GameUnitMgr>().MainHero;
            txt_NetUrl.text = $"NetUrl:{mainHero.NetUrl}";
        }
        private void OnUpdateHandler()
        {
            var mainPlayer = GetModule<GameUnitMgr>().MainHero;
            if (mainPlayer != null)
            {
                txt_Pos.text = "Pos:" + mainPlayer.transform.position;
            }
            txt_Delay.text = "Delay:" + GetModule<HeartBeatMgr>().NetDelay;
        }
    }
}
