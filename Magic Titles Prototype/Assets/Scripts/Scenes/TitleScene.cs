using System;
using Apps.Runtime.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.Runtime.Scenes
{
    public sealed class TitleScene : MonoBehaviour
    {
        [SerializeField] Button _bpm70Button;
        [SerializeField] Button _bpm120Button;
        [SerializeField] Slider _volumnSlider;

        AudioSource _source;
        private void Start()
        {
            _source = Loader.Instance.AudioSource;

            _bpm70Button.onClick.AddListener(OnBPM70);
            _bpm120Button.onClick.AddListener(OnBPM120);

            _volumnSlider.value = _source.volume;
            _volumnSlider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnBPM70() => Load(0);
        
        private void OnBPM120() => Load(1);
        
        private void OnValueChanged(float volumn) => Loader.Instance.AudioSource.volume = volumn;
        
        private void Load(int id)
        {
            Loader.Instance.Transition.Load(Scene.Gameplay, id);
        }
    }
}