using Apps.Runtime.Common;
using Apps.Runtime.Interators;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.Runtime.Scenes
{
    public sealed class GameplayScene : MonoBehaviour
    {
        [SerializeField] Gameplay _gameplay;
        [SerializeField] LevelDesign _levelDesign;
        [SerializeField] Button _titleButton;

        private void Start()
        {
            Initialize(Loader.Instance.Transition.TransitionData);
            _titleButton.onClick.AddListener(LoadTitle);
        }

        private void LoadTitle()
        {
            Loader.Instance.Transition.Load(Scene.Title);
        }

        // get audio and game stats from database.
        private void Initialize(object transitionData)
        {
            var levelDesign = _levelDesign.GetData(transitionData != null ? (int)transitionData : 1);
            var source = Loader.Instance.AudioSource;

            // change clip according to settings.
            if (source.clip != levelDesign.AudioClip)
            {
                source.clip = levelDesign.AudioClip;
            }

            // restart playback.
            source.time = 0;
            source.Play();

            _gameplay.Initialize(levelDesign, source);
        }
    }
}