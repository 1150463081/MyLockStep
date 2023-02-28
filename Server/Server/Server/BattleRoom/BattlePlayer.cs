using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public class BattlePlayer
    {
        public long Url { get; private set; }
        public void Init()
        {
            Url = GenerateSrl();
        }
        private long GenerateSrl()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}
