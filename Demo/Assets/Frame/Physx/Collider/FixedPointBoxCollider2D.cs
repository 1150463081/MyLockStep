using System.Collections.Generic;
using LockStepFrame;

namespace SimplePhysx
{
    public class FixedPointBoxCollider2D : FixedPointCollider2DBase, IBoxShape
    {
        public float editLength;
        public float editWidth;
        //长宽高
        public FXInt Length { get; protected set; }
        public FXInt Width { get; protected set; }
        //三个轴的单位向量
        public FXVector3 XAxis { get; protected set; }
        public FXVector3 ZAxis { get; protected set; }

        public void Init(FXVector3 pos, FXInt length, FXInt width, FXVector3 xAxis, FXVector3 yAxis, FXVector3 zAxis)
        {
            Pos = pos;
            Length = length;
            Width = width;
            XAxis = xAxis;
            ZAxis = zAxis;
        }
        public override bool DetectBoxCollider(IBoxShape boxShape, ref FXVector3 normal, ref FXVector3 adjust)
        {
            //获取边界与顶点
            var vertexs = Utility.Shape.GetVertexs(this);
            var borders = Utility.Shape.GetBorders(vertexs);
            var vertexs_2 = Utility.Shape.GetVertexs(boxShape);
            var borders_2 = Utility.Shape.GetBorders(vertexs_2);

            FXInt min = int.MaxValue;
            FXVector3 minNormal = FXVector3.zero;
            List<FXVector3> allBorders = new List<FXVector3>();
            allBorders.AddRange(borders);
            allBorders.AddRange(borders_2);
            //todo 分离轴算法
            FXVector3 minAdjustNormal = FXVector3.zero;
            for (int i = 0; i < allBorders.Count; i++)
            {
                //挑一条轴，取他的正交向量做为投影向量
                FXVector3 pNormal = FXVector3.Cross(allBorders[i], new FXVector3(0, 1, 0)).normalized;
                //获取两个多边形的两组顶点在投影向量上的两端投影点
                var mPoints = GetProjectedPoints(pNormal, vertexs);
                var oPoints = GetProjectedPoints(pNormal, vertexs_2);
                //检测两段投影是否有重合
                var adjustNormal = FXVector3.zero;
                var a = GetMaxDis(mPoints, oPoints, ref adjustNormal);
                var b = (mPoints[1] - mPoints[0]).magnitude + (oPoints[1] - oPoints[0]).magnitude;
                var c = a - b;
                if (c > 0)//没有重合
                {
                    adjust = FXVector3.zero;
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
            normal = -adjust.normalized;
            return true;
        }

        public override bool DetectSphereCollider(ISphereShape sphereShape, ref FXVector3 normal, ref FXVector3 adjust)
        {
            if (sphereShape is FixedPointSphereCollider2D col)
            {
                FXVector3 tmpNormal = FXVector3.zero;
                FXVector3 tmpAdjust = FXVector3.zero;
                bool result = col.DetectBoxCollider(this, ref tmpNormal, ref tmpAdjust);
                normal = -tmpNormal;
                adjust = -tmpAdjust;
                return result;
            }
            return false;
        }


    }
}
