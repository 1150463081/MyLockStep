using SimplePhysx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockStepFrame
{
    public partial class Utility
    {
        public class Shape
        {
            public static FXVector3[] GetVertexs(IBoxShape boxShape)
            {
                var vertexs = new FXVector3[4];
                vertexs[0] = boxShape.Pos + boxShape.XAxis * boxShape.Length / 2 + boxShape.ZAxis * boxShape.Width / 2;
                vertexs[1] = boxShape.Pos + boxShape.XAxis * boxShape.Length / 2 - boxShape.ZAxis * boxShape.Width / 2;
                vertexs[2] = boxShape.Pos - boxShape.XAxis * boxShape.Length / 2 - boxShape.ZAxis * boxShape.Width / 2;
                vertexs[3] = boxShape.Pos - boxShape.XAxis * boxShape.Length / 2 + boxShape.ZAxis * boxShape.Width / 2;
                return vertexs;
            }
            public static FXVector3[] GetBorders(FXVector3[] vertexs)
            {
                var borders = new FXVector3[4];
                borders[0] = vertexs[1] - vertexs[0];
                borders[1] = vertexs[2] - vertexs[1];
                borders[2] = vertexs[3] - vertexs[2];
                borders[3] = vertexs[0] - vertexs[3];
                return borders;
            }
        }
    }
}
