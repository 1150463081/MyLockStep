using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets.Kcp;
using System.Buffers;

namespace KCPNet
{
    public class KCPHandle : IKcpCallback
    {
        public Action<Memory<byte>> OutEvent;
        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            using (buffer)
            {
                OutEvent?.Invoke(buffer.Memory.Slice(0, avalidLength));
            }
        }

        public Action<byte[]> RecvEvent;
        public void Recive(byte[] buffer)
        {
            RecvEvent?.Invoke(buffer);
        }
    }
}
