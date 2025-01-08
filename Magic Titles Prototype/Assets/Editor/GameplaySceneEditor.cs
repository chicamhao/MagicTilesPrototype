using Apps.Runtime.Scenes;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(GameplayScene))]
    public sealed class GameplaySceneEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Reload"))
            {
                (target as GameplayScene).Reload();
            }
        }
    }
}