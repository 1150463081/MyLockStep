using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplePhysx;
using PEMath;

public class FP_BoxCol2DComponent : MonoBehaviour
{
    public float Length;
    public float Width;
    public float YRotation;

    public FixedPointBoxCollider2D boxCol;

    private void Awake()
    {
        boxCol = new FixedPointBoxCollider2D();
        var rotation = new Vector3(0,YRotation, 0);
        var right = Quaternion.Euler(rotation) * transform.right;
        var forward = Quaternion.Euler(rotation) * transform.forward;
        var up = Quaternion.Euler(rotation) * transform.up;
        boxCol.Init(new PEVector3(transform.position), new PEInt(Length), new PEInt(Width), new PEVector3(right), new PEVector3(forward), new PEVector3(up));
    }
}
