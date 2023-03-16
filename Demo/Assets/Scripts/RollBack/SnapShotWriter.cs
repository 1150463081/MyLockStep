using System.IO;
using LockStepFrame;

namespace GameCore
{
    /// <summary>
    /// 快照写入
    /// </summary>
    public class SnapShotWriter
    {
        private MemoryStream stream;
        //private FastBinnayWriter writer;
        private BinaryWriter writer;

        public SnapShotWriter()
        {
            //writer = new FastBinnayWriter();
        }
        public void Init(MemoryStream ms)
        {
            stream = ms;
            //writer.Init(ms);
            
            writer = new BinaryWriter(ms);        
        }

        public void Write(FXVector3 value)
        {
            writer.Write(value.x.ScaledValue);
            writer.Write(value.y.ScaledValue);
            writer.Write(value.z.ScaledValue);
        }
        public void Write(FXInt value)
        {
            writer.Write(value.ScaledValue);
        }
        public void Flush()
        {
            writer.Flush();
        }
    }
}
