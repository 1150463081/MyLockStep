using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCore
{
    public interface IRollBack
    {
        public void TakeSnapShot(SnapShotWriter writer);
        public void RollBackTo(SnapShotReader reader);
    }
}
