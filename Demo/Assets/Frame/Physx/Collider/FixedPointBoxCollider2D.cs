using System;
using System.Collections.Generic;
using System.Text;
using PEMath;
using UnityEngine;

namespace SimplePhysx
{
    public class FixedPointBoxCollider2D : FixedPointCollider2DBase
    {
        //长宽高
        public PEVector3 Size { get; protected set; }
        //三个轴的单位向量
        public PEVector3 XAxis { get; protected set; }
        public PEVector3 YAxis { get; protected set; }
        public PEVector3 ZAxis { get; protected set; }

        public void Init(PEVector3 pos, PEVector3 size, PEVector3 xAxis, PEVector3 yAxis, PEVector3 zAxis)
        {
            Pos = pos;
            Size = size;
            XAxis = xAxis;
            YAxis = yAxis;
            ZAxis = zAxis;
            //DrawLine(Vertexs);
        }
        public override bool DetectBoxCollider(FixedPointBoxCollider2D boxCol, ref PEVector3 normal, ref PEVector3 adjust)
        {
            //获取边界与顶点
            var vertexs = GetVertexs();
            var borders = GetBorders(vertexs);
            var vertexs_2 = boxCol.GetVertexs();
            var borders_2 = boxCol.GetBorders(vertexs_2);

            PEInt min = int.MaxValue;
            PEVector3 minNormal = PEVector3.zero;
            List<PEVector3> allBorders = new List<PEVector3>();
            allBorders.AddRange(borders);
            allBorders.AddRange(borders_2);
            //todo 分离轴算法
            PEVector3 minAdjustNormal = PEVector3.zero;
            for (int i = 0; i < allBorders.Count; i++)
            {
                //挑一条轴，取他的正交向量做为投影向量
                PEVector3 pNormal = PEVector3.Cross(allBorders[i], new PEVector3(0, 1, 0)).normalized;
                //获取两个多边形的两组顶点在投影向量上的两端投影点
                var mPoints = GetProjectedPoints(pNormal, vertexs);
                var oPoints = GetProjectedPoints(pNormal, vertexs_2);
                //检测两段投影是否有重合
                var adjustNormal = PEVector3.zero;
                var a = GetMaxDis(mPoints, oPoints, ref adjustNormal);
                var b = (mPoints[1] - mPoints[0]).magnitude + (oPoints[1] - oPoints[0]).magnitude;
                var c = a - b;
                if (c > 0)//没有重合
                {
                    adjust = PEVector3.zero;
                    return false;
                }
                if (c.Abs < min)
                {
                    min = c.Abs;
                    minNormal = pNormal;
                    minAdjustNormal = adjustNormal;
                }
            }
            adjust = min * minAdjustNormal;
            return true;
        }

        public override bool DetectSphereCollider(FixedPointSphereCollider2D sphereCol, ref PEVector3 normal, ref PEVector3 adjust)
        {
            PEVector3 tmpNormal = PEVector3.zero;
            PEVector3 tmpAdjust = PEVector3.zero;
            bool result = sphereCol.DetectBoxCollider(this, ref tmpNormal, ref tmpAdjust);
            normal = -tmpNormal;
            adjust = -tmpAdjust;
            return result;
        }


        public PEVector3[] GetVertexs()
        {
            var vertexs = new PEVector3[4];
            vertexs[0] = Pos + XAxis * Size.x / 2 + ZAxis * Size.z / 2;
            vertexs[1] = Pos + XAxis * Size.x / 2 - ZAxis * Size.z / 2;
            vertexs[2] = Pos - XAxis * Size.x / 2 - ZAxis * Size.z / 2;
            vertexs[3] = Pos - XAxis * Size.x / 2 + ZAxis * Size.z / 2;
            return vertexs;
        }
        public PEVector3[] GetBorders(PEVector3[] vertexs)
        {
            var borders = new PEVector3[4];
            borders[0] = vertexs[1] - vertexs[0];
            borders[1] = vertexs[2] - vertexs[1];
            borders[2] = vertexs[3] - vertexs[2];
            borders[3] = vertexs[0] - vertexs[3];
            return borders;
        }


    }
}
