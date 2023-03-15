using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;

namespace GameCore
{
    /// <summary>
    /// 快照写入
    /// </summary>
    public class SnapShotWriter
    {
        private MemoryStream stream;
        private FastBinnayWriter writer;

        public SnapShotWriter()
        {
            writer = new FastBinnayWriter();
        }
        public void Init(MemoryStream ms)
        {
            stream = ms;
            writer.Init(ms);
        }

        public void Write(PEVector3 value)
        {
            writer.Write(value.x.ScaledValue);
            writer.Write(value.y.ScaledValue);
            writer.Write(value.z.ScaledValue);
        }
        public void Write(PEInt value)
        {
            writer.Write(value.ScaledValue);
        }
        public void Flush()
        {
            writer.Flush();
        }
    }
}
