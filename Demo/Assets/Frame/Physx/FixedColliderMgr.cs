using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;
using SimplePhysx;
using UnityEngine;

namespace SimplePhysx
{
    [Module]
    public partial class FixedColliderMgr : Module
    {
        private List<FixedPointCollider2DBase> fpColList = new List<FixedPointCollider2DBase>();
        private QuadTree quadTree;

        public override void OnInit()
        {
            base.OnInit();
            quadTree = new QuadTree(10);
            InitEnvCollider();
        }

        public void Register(IFixedPointCol2DComponent colComp)
        {
            Debug.Log($"注册col");
            colComp.InitCollider();
            fpColList.Add(colComp.Col);
            colComp.Col.OnChangePos += quadTree.OnColPosChange;
            quadTree.OnColPosChange(colComp.Col);
        }
        public void DeRegister(IFixedPointCol2DComponent colComp)
        {
            fpColList.Remove(colComp.Col);
            colComp.Col.OnChangePos -= quadTree.OnColPosChange;
        }
        public void CalcCollison(IFixedPointCol2DComponent colComp, FXVector3 moveDir)
        {
            var adjust = FXVector3.zero;

            //获取需要检测的碰撞体
            List<FixedPointCollider2DBase> checkColList = new List<FixedPointCollider2DBase>();
            var checkCells = colComp.Col.QuadCells;
            for (int i = 0; i < checkCells.Count; i++)
            {
                for (int j = 0; j < checkCells[i].ColList.Count; j++)
                {
                    if (!checkColList.Contains(checkCells[i].ColList[j]))
                    {
                        checkColList.Add(checkCells[i].ColList[j]);
                    }
                }
            }


            colComp.Col.ClacCollison(checkColList, ref moveDir, ref adjust);
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
