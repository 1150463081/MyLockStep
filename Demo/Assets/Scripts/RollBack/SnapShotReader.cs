using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GameCore
{
    public class SnapShotReader
    {
        private MemoryStream ms;
       public int ReadInt()
        {
            using (BinaryReader reader = new BinaryReader(ms))
            {
                int value= reader.ReadInt32();
                return value;
            }
        }


    }
}
