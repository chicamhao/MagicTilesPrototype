using UnityEditor;
using UnityEngine;
using Apps.Runtime.Domains.WebGL;

namespace Editor
{
    [CustomEditor(typeof(WebGLHandler))]
    public class WebGLHandlerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Save"))
            {
                (target as WebGLHandler).Save();
            }
        }
    }
}