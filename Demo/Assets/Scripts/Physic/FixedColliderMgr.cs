using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;
using SimplePhysx;
using UnityEngine;

namespace GameCore
{
    [Module]
    public class FixedColliderMgr : Module
    {
        private List<FixedPointCollider2DBase> fpColList = new List<FixedPointCollider2DBase>();

        public override void OnInit()
        {
            base.OnInit();
            InitEnvCollider();
        }

        public void Register(IFixedPointCol2DComponent colComp)
        {
            Debug.Log($"注册col");
            colComp.InitCollider();
            fpColList.Add(colComp.Col);
        }
        public void DeRegister(IFixedPointCol2DComponent colComp)
        {

        }
        public void CalcCollison(IFixedPointCol2DComponent colComp, FXVector3 moveDir)
        {
            var adjust = FXVector3.zero;
            colComp.Col.ClacCollison(fpColList, ref moveDir, ref adjust);
            if (moveDir != FXVector3.zero)
            {
                Debug.Log($"{adjust}");
                var logicPos = colComp.Col.Pos + adjust;
                colComp.Col.SetPos(logicPos);
            }

        }
        public void InitEnvCollider()
        {
            var root = GameObject.Find("Env");
            var components = root.GetComponentsInChildren<IFixedPointCol2DComponent>();
            for (int i = 0; i < components.Length; i++)
            {
                Register(components[i]);
            }
        }
    }
}
