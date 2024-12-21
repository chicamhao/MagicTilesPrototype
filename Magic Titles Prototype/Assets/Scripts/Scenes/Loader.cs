using UnityEngine;

namespace Apps.Runtime.Scenes
{
    public sealed class Loader : MonoBehaviour
    {
        public static Loader Instance { get; private set; }

        public Transition Transition => _transition;
        Transition _transition;

        void Awake()
        {
            // TODO DI
            Instance = this;
            DontDestroyOnLoad(this);

            _transition = new Transition();
        }
    }
}