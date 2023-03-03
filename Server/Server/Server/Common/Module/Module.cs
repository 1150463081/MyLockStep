using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public abstract class Module
    {
        public abstract void Init();
        public abstract void Update();
    }
}
