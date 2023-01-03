using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SimplePhysx;

[CustomEditor(typeof(FP_SphereCol2DComponent))]
public class SphereColliderEditor : Editor
{
    private void OnSceneGUI()
    {
        var sphereCol = target as FP_SphereCol2DComponent;

        EditorGUI.BeginChangeCheck();
        Handles.color = Color.green;
        float areaOfEffect = Handles.RadiusHandle(Quaternion.identity, sphereCol.transform.position, sphereCol.radius);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Area Of Effect");
            sphereCol.radius = areaOfEffect;
        }

    }
}
