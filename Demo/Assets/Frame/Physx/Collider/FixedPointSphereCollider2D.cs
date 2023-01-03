using PEMath;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimplePhysx
{
    public class FixedPointSphereCollider2D : FixedPointCollider2DBase
    {

        public PEInt Radius { get; protected set; }


        public void Init(PEVector3 pos, float radius)
        {
            Pos = pos;
            Radius = new PEInt(radius);
        }

        public override bool DetectSphereCollider(FixedPointSphereCollider2D sphereCol, ref PEVector3 normal, ref PEVector3 adjust)
        {
            var dis = (Pos - sphereCol.Pos).magnitude;
            if (dis > Radius + sphereCol.Radius)
            {
                return false;
            }
            else
            {
                normal = (Pos - sphereCol.Pos).normalized;
                adjust = normal * (Radius + sphereCol.Radius - dis);
                return true;
            }
        }

        public override bool DetectBoxCollider(FixedPointBoxCollider2D boxCol, ref PEVector3 normal, ref PEVector3 adjust)
        {
            #region 老的检测方法，速度过快会不准确
            ////矩形中心到圆心的向量bo
            //PEVector3 bo = Pos - boxCol.Pos;
            ////计算bo在x轴上的投影和z轴的投影
            //PEInt dotX = PEVector3.Dot(bo, boxCol.XAxis);
            //PEInt dotZ = PEVector3.Dot(bo, boxCol.ZAxis);
            ////限制投影的长度在盒子的长宽范围内
            //dotX = PECalc.Clamp(dotX, -boxCol.Size.x / 2, boxCol.Size.x / 2);
            //dotZ = PECalc.Clamp(dotZ, -boxCol.Size.z / 2, boxCol.Size.z / 2);
            ////圆形离矩形最近点u
            //PEVector3 u = boxCol.Pos + dotX * boxCol.XAxis + dotZ * boxCol.ZAxis;
            ////u到圆心的向量
            //PEVector3 uo = Pos - u;
            //if (uo.magnitude > Radius)
            //{
            //    return false;
            //}
            //else
            //{
            //    normal = uo.normalized;
            //    adjust = normal * Radius - uo;
            //    return true;
            //}
            #endregion
            #region 分离轴算法检测
            //方向和圆形盒子所有投影向量集合
            List<PEVector3> pNormalLst = new List<PEVector3>();
            //获取方形的投影向量
            var boxVertexs = boxCol.GetVertexs();
            var boxBorders = boxCol.GetBorders(boxVertexs);
            var pNormal = PEVector3.zero;
            for (int i = 0; i < boxBorders.Length; i++)
            {
                pNormal = PEVector3.Cross(boxBorders[i], new PEVector3(0, 1, 0)).normalized;
                pNormalLst.Add(pNormal);
            }
            //获取圆形的投影向量
            //圆形比较特殊，圆的投影向量就是途经圆心和多边形上离圆心最近的顶点的直线。
            PEInt minDis = (PEInt)int.MaxValue;
            for (int i = 0; i < boxVertexs.Length; i++)
            {
                var dis = (Pos - boxVertexs[i]).magnitude;
                if (dis < minDis)
                {
                    minDis = dis;
                    pNormal = Pos - boxVertexs[i];
                }
            }
            pNormalLst.Add(pNormal);

            PEInt min = int.MaxValue;
            PEVector3 minNormal = PEVector3.zero;
            PEVector3 minAdjustNormal = PEVector3.zero;

            for (int i = 0; i < pNormalLst.Count; i++)
            {
                pNormal = pNormalLst[i];
                var mPoints = GetSphereProjectPoint(pNormal);
                var oPoints = GetProjectedPoints(pNormal, boxVertexs);
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
            #endregion
        }
        /// <summary>
        /// 获取圆形在投影向量上的投影点
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        private PEVector3[] GetSphereProjectPoint(PEVector3 normal)
        {
            PEVector3[] projections = new PEVector3[2];
            var project = PEVector3.Dot(Pos, normal);
            projections[0] = (project - Radius) * normal;
            projections[1] = (project + Radius) * normal;
            return projections;
        }
    }
}
