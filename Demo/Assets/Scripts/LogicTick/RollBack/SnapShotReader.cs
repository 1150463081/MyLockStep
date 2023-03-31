using System.IO;
using LockStepFrame;

namespace GameCore
{
    public class SnapShotReader
    {
        private MemoryStream ms;
        //private FastBinnayReader reader;
        private BinaryReader reader;

        public SnapShotReader()
        {
            //reader = new FastBinnayReader();
        }

        public void Init(MemoryStream stream)
        {
            ms = stream;
            //reader.Init(stream);
            reader = new BinaryReader(ms);
        }


        public FXVector3 ReadFXVector3()
        {
            FXInt xValue = new FXInt();
            FXInt yValue = new FXInt();
            FXInt zValue = new FXInt();
            xValue.ScaledValue = reader.ReadInt64();
            yValue.ScaledValue = reader.ReadInt64();
            zValue.ScaledValue = reader.ReadInt64();
            return new FXVector3(xValue, yValue, zValue);
        }


    }
}
