using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimplePhysx;

[CustomEditor(typeof(FP_SphereCol2DComponent))]
public class SphereCollider2DEditor : Editor
{
    private void OnSceneGUI()
    {
        var sphereCol = target as FP_SphereCol2DComponent;
        Handles.color = Color.green;
        Handles.CircleHandleCap(1, sphereCol.transform.position+Vector3.up*sphereCol.GizmosHeight, Quaternion.LookRotation(Vector3.up), sphereCol.radius, EventType.Repaint);
    }
}
