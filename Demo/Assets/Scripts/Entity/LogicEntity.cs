using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LockStepFrame;
using SimplePhysx;

namespace GameCore
{
    public class LogicEntity : EntityBase
    {
        protected PEVector3 InputDir;
        protected IFixedPointCol2DComponent ColComp;
        protected FixedPointCollider2DBase Col;

        public override void OnLoadResComplete()
        {
            base.OnLoadResComplete();
            ColComp = gameObject.GetComponentInChildren<FP_BoxCol2DComponent>();
            ColComp.InitCollider();
            Col = ColComp.Col;
        }
        public virtual void LogicTick()
        {
            TickMove();
        }
        public virtual void InputMove(PEVector3 dir)
        {
            InputDir = dir;
        }
        public virtual void InputSkill()
        {

        }
        protected virtual void TickMove()
        {
            Col.Pos += InputDir * 5 * ((PEInt)0.66f);
            transform.position = Col.Pos.ConvertViewVector3();
        }
    }
}
