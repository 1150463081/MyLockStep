using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;

namespace GameCore
{
    [Module]
    public class UpdateMgr:Module
    {
        public Action UpdateEvent;

        public override void OnUpdate()
        {
            UpdateEvent?.Invoke();
        }
    }
}
