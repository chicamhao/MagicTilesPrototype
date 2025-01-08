using UnityEditor;
using UnityEngine;

namespace Apps.Runtime.Scenes
{
    public sealed class Loader : MonoBehaviour
    {
        public static Loader Instance { get; private set; }

        public AudioSource AudioSource => _audioSource;
        [SerializeField] AudioSource _audioSource;

        public Transition Transition => _transition;
        Transition _transition;

        void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);

            _transition = new Transition();
        }
    }
}