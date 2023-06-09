using LockStepFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePhysx
{
    public interface IShape
    {
        public FXVector3 Pos { get; }
    }
    public interface IBoxShape : IShape
    {
        //长宽高
        public FXInt Length { get; }
        public FXInt Width { get; }
        //三个轴的单位向量
        public FXVector3 XAxis { get; }
        public FXVector3 ZAxis { get; }
    }
    public interface ISphereShape : IShape
    {
        public FXInt Radius { get; }
    }
}
