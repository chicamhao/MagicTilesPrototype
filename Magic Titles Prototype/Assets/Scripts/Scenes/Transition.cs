using System;
using UnityEngine.SceneManagement;

namespace Apps.Runtime.Scenes
{
    public sealed class Transition
    {
        public object TransitionData => _transitionData;
        object _transitionData;

        public void Load(Scene scene, object data = null)
        {
            SceneManager.LoadScene(scene switch
            {
                Scene.Title => SceneName.Title,
                Scene.Gameplay => SceneName.Playground,
                _ => throw new NotImplementedException($"{scene}")
            });
            _transitionData = data;
        }

        public static void Exit()
        {
            UnityEngine.Application.Quit();
        }
    }

    public enum Scene
    {
        Title,
        Gameplay,
    }

    public sealed class SceneName
    {
        public static string Title = "title";
        public static string Playground = "gameplay";
    }
}