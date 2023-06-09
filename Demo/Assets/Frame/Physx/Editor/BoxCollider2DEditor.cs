using SimplePhysx;
using UnityEditor;
using UnityEngine;

namespace GameCore
{
    [CustomEditor(typeof(FP_BoxCol2DComponent))]
    public class BoxCollider2DEditor : Editor
    {
        private void OnSceneGUI()
        {
            var boxCol = target as FP_BoxCol2DComponent;
            var vertexs = GetVertexs();
            Handles.color = Color.green;
            Handles.DrawLines(new Vector3[] { vertexs[0], vertexs[1] });
            Handles.DrawLines(new Vector3[] { vertexs[1], vertexs[2] });
            Handles.DrawLines(new Vector3[] { vertexs[2], vertexs[3] });
            Handles.DrawLines(new Vector3[] { vertexs[3], vertexs[0] });
        }
        private Vector3[] GetVertexs()
        {
            var boxCol = target as FP_BoxCol2DComponent;
            var vertexs = new Vector3[4];
            var rotation = new Vector3(0, boxCol.YRotation, 0);
            var right = Quaternion.Euler(rotation) * boxCol.transform.right;
            var forward = Quaternion.Euler(rotation) * boxCol.transform.forward;
            var up = Quaternion.Euler(rotation) * boxCol.transform.up;
            vertexs[0] = boxCol.transform.position + right * boxCol.Length / 2 + forward * boxCol.Width / 2;
            vertexs[1] = boxCol.transform.position + right * boxCol.Length / 2 - forward * boxCol.Width / 2;
            vertexs[2] = boxCol.transform.position - right * boxCol.Length / 2 - forward * boxCol.Width / 2;
            vertexs[3] = boxCol.transform.position - right * boxCol.Length / 2 + forward * boxCol.Width / 2;
            for (int i = 0; i < vertexs.Length; i++)
            {
                vertexs[i] += up * boxCol.GizmosHeight;
            }
            return vertexs;
        }
    }
}
