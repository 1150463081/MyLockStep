﻿using System;
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
    public class LogicEntity : EntityBase,IRollBack
    {
        protected IFixedPointCol2DComponent ColComp;
        protected FixedPointCollider2DBase Col;

       

        #region
        //移动朝向
        protected FXVector3 MoveDir;
        #endregion

        public override void OnInit()
        {
            base.OnInit();
            ModuleManager.Instance.GetModule<RollBackMgr>().Register(this);
        }
        public override void OnLoadResComplete()
        {
            base.OnLoadResComplete();
            ColComp = gameObject.GetComponentInChildren<FP_BoxCol2DComponent>();
            ColComp.InitCollider();
            Col = ColComp.Col;
        }
       


        //逻辑帧随服务器消息更新
        #region 逻辑帧更新

        public void ServerLogicTick()
        {

        }
        public void ClientLogicTick()
        {
            LogicTickMove();
        }
        protected void LogicTickMove()
        {
            //逻辑位置更新
            Col.Pos += MoveDir * BaseVO.MoveSpeed * ((FXInt)0.66f);
            //transform.position = Col.Pos.ConvertViewVector3();
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
            var tgtPos= Col.Pos.ConvertViewVector3();
            transform.position = Vector3.Lerp(transform.position, tgtPos,.5f);
        }
        #endregion 
        
        

        public void TakeSnapShot(SnapShotWriter writer)
        {
            writer.Write(Col.Pos);
        }

        public void RollBackTo(SnapShotReader reader)
        {
            Col.Pos = reader.ReadFXVector3();
        }
    }
}
