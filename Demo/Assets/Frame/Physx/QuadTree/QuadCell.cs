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
    public class QuadCell : IBoxShape
    {
        public string Key { get; private set; }
        public int XIndex { get; private set; }
        public int ZIndex { get; private set; }

        public FXInt Length { get; private set; }

        public FXInt Width { get; private set; }

        public FXVector3 XAxis { get; private set; }

        public FXVector3 ZAxis { get; private set; }

        public FXVector3 Pos { get; private set; }

        public List<FixedPointCollider2DBase> ColList { get; private set; } = new List<FixedPointCollider2DBase>();

        private GameObject debugObj;

        public void EnterCell(FixedPointCollider2DBase col)
        {
            ColList.Add(col);
        }
        public void ExitCell(FixedPointCollider2DBase col)
        {
            ColList.Remove(col);
        }
        public QuadCell(int xIndex, int zIndex, FXInt length)
        {
            Key = QuadTree.GetCellKey(xIndex, zIndex);
            XIndex = xIndex;
            ZIndex = zIndex;
            Length = length;
            Width = length;
            XAxis = FXVector3.right;
            ZAxis = FXVector3.forward;
            FXInt x = (xIndex - 1) * Length + Length / 2;
            FXInt z = (zIndex - 1) * Length + Length / 2;
            Pos = new FXVector3(x, 0, z);

            

        }
        private void InitCellObj()
        {
            var prefab = Resources.Load<GameObject>("QuadCell");
        }
    }
}
