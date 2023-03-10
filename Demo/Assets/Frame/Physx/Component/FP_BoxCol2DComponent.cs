using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplePhysx;
using LockStepFrame;

namespace GameCore
{
    public class FP_BoxCol2DComponent : MonoBehaviour, IFixedPointCol2DComponent
    {
        public float Length;
        public float Width;
        public float YRotation;
        public float GizmosHeight;

        public FixedPointBoxCollider2D boxCol;

        public FixedPointCollider2DBase Col => boxCol;

        public void InitCollider()
        {
            boxCol = new FixedPointBoxCollider2D();
            var rotation = new Vector3(0, YRotation, 0);
            var right = Quaternion.Euler(rotation) * transform.right;
            var forward = Quaternion.Euler(rotation) * transform.forward;
            var up = Quaternion.Euler(rotation) * transform.up;
            boxCol.Init(new PEVector3(transform.position), new PEInt(Length), new PEInt(Width), new PEVector3(right), new PEVector3(forward), new PEVector3(up));
        }
    }
}
