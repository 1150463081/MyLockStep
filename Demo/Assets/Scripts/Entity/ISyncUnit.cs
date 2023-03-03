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
        public void InputKey(OpKey opKey);
        
    }
}
