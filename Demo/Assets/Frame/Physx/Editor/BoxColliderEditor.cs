using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimplePhysx;

[CustomEditor(typeof(FixedPointBoxCollider2D))]
public class BoxColliderEditor : Editor
{
    private void OnSceneGUI()
    {
        Handles.SphereHandleCap(1,Vector3.zero,Quaternion.identity,3,EventType.MouseDrag);
    }
}
