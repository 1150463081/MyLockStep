using SimplePhysx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LockStepFrame;

namespace GameCore
{
    public class FP_SphereCol2DComponent : MonoBehaviour, IFixedPointCol2DComponent
    {
        public float radius;
        public float GizmosHeight;
        public FixedPointSphereCollider2D SphereCol { get; private set; }

        public FixedPointCollider2DBase Col => SphereCol;

        public void InitCollider()
        {
            SphereCol = new FixedPointSphereCollider2D();
            SphereCol.Init(new PEVector3(transform.position), radius);
        }
    }
}
