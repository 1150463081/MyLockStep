using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;
using SimplePhysx;
using UnityEngine;
using NetProtocol;

namespace GameCore
{
    public class LogicEntity : EntityBase, IRollBack
    {
        protected IFixedPointCol2DComponent ColComp;
        private PathPointUtil pathPointUtil;


        #region
        //移动朝向
        protected FXVector3 MoveDir;
        #endregion

        public override void OnInit()
        {
            base.OnInit();
            ModuleManager.Instance.GetModule<RollBackMgr>().Register(this);
            GameEvent.LockStep.ClientLogicTickOver += OnClientFrameOverHandler;
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            ModuleManager.Instance.GetModule<RollBackMgr>().DeRegister(this);
            GameEvent.LockStep.ClientLogicTickOver -= OnClientFrameOverHandler;
        }
        public override void OnLoadResComplete()
        {
            base.OnLoadResComplete();
            pathPointUtil = new PathPointUtil();
            ColComp = gameObject.GetComponentInChildren<FP_BoxCol2DComponent>();
            ModuleManager.Instance.GetModule<FixedColliderMgr>().Register(ColComp);
        }
        protected virtual void OnClientFrameOverHandler(int cFrameId)
        {
            MoveDir = FXVector3.zero;
        }


        //逻辑帧随服务器消息更新
        #region 逻辑帧更新

        public void ServerLogicTick()
        {

        }
        public void ClientLogicTick(int frameId)
        {
            LogicTickMove(frameId);
        }
        protected void LogicTickMove(int frameId)
        {

            //逻辑位置更新
            var pos = ColComp.Col.Pos + MoveDir * BaseVO.MoveSpeed * ((FXInt)0.066f);
            ColComp.Col.SetPos(pos);
            var fcMgr = ModuleManager.Instance.GetModule<FixedColliderMgr>();
            fcMgr.CalcCollison(ColComp, MoveDir);

            pathPointUtil.AddPathPoint(ColComp.Col.Pos);

        }
        #endregion
        #region 表现层本地轮询
        //表现层更新，本地每帧轮询
        public void ViewTick()
        {
            ViewTickMove();
        }
        protected void ViewTickMove()
        {

            var tgtPos = ColComp.Col.Pos.ConvertViewVector3();
            if (tgtPos == transform.position)
            {
                return;
            }
            var offest = tgtPos - transform.position;
            var moveDir = offest.normalized;
            var logicSpeed = ModuleManager.Instance.GetModule<LogicTickMgr>().LogicSpeed;
            var moveDis = moveDir * BaseVO.MoveSpeed.RawFloat * Time.deltaTime * logicSpeed;
            if (moveDis.magnitude > offest.magnitude)
            {
                transform.position = tgtPos;
            }
            else
            {
                transform.position += moveDis;
            }
        }
        #endregion



        public void TakeSnapShot(SnapShotWriter writer)
        {
            writer.Write(ColComp.Col.Pos);
        }

        public void RollBackTo(SnapShotReader reader)
        {
            ColComp.Col.SetPos(reader.ReadFXVector3());
        }
    }
}
