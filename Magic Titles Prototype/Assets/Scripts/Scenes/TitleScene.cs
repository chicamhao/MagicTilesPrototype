using UnityEngine;
using UnityEngine.UI;

namespace Apps.Runtime.Scenes
{
    public sealed class TitleScene : MonoBehaviour
    {
        [SerializeField] Button _closeButton;
        [SerializeField] Button _bpm70Button;
        [SerializeField] Button _bpm120Button;

        private void Awake()
        {
            _closeButton.onClick.AddListener(OnClosed);
            _bpm70Button.onClick.AddListener(OnBPM70);
            _bpm120Button.onClick.AddListener(OnBPM120);
        }

        private void OnBPM70()
        {
            Loader.Instance.Transition.Load(Scene.Gameplay, 0);
        }

        private void OnBPM120()
        {
            Loader.Instance.Transition.Load(Scene.Gameplay, 1);
        }

        private void OnClosed()
        {
            Transition.Exit();
        }
    }
}