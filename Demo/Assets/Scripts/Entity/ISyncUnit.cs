using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetProtocol;

namespace GameCore
{
    public interface ISyncUnit
    {
        public uint NetUrl { get; }
        public bool CheckOutOpKey(int sFrameId, OpKey opKey);
        public void ReleaseOperateInfo(int frameId);
        public void ReplaceOperateInfo(int frameId, OpKey opKey);
    }
}
