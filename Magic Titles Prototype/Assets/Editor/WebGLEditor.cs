using UnityEditor;

namespace Editor
{
    public static class WebGLEditor
    {
        [MenuItem("WebGL/Enable Embedded Resources")]
        public static void EnableEmbeddedResources()
        {
            PlayerSettings.WebGL.useEmbeddedResources = true;
        }
    }
}