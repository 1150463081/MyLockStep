using SimplePhysx;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PEMath;

public class FP_SphereCol2DComponent : MonoBehaviour
{
    public float radius;

    public FixedPointSphereCollider2D SphereCol { get; private set; }
    private void Awake()
    {
        SphereCol = new FixedPointSphereCollider2D();
        SphereCol.Init(new PEVector3(transform.position), radius);
    }
}
