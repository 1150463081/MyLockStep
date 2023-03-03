using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimplePhysx;

namespace GameCore
{
    public interface IFixedPointCol2DComponent
    {
        public FixedPointCollider2DBase Col { get; }
        public void InitCollider();
    }
}
