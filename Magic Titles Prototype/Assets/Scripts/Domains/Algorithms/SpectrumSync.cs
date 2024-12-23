using System;
using Apps.Runtime.Common;
using Apps.Runtime.Domains.WebGL;
using UnityEngine;
using UnityEngine.Assertions;

namespace Apps.Runtime.Domains.Algorithms
{
    public abstract class SpectrumSync : MonoBehaviour, ISync
    {
        [SerializeField] FFTWindow _spectrumWindowType = FFTWindow.Rectangular;
        readonly float[] _spectrumData = new float[512]; // note: could be a performance impact?

        protected AudioSource _source;
        LevelDesignData _levelDesign;
        WebGLHandler _webHandler;

        bool _initialized;
        float _timeToMiddle;
        float _lastSpawnTime;

        Action _spawnAction;
        Action<float> _onAmplitudeChanged;

        protected float _volumn => _source.volume;

        private void Awake()
        {
            _webHandler = GetComponent<WebGLHandler>();
        }

        public void Initialize(
            AudioSource audioSource, LevelDesignData levelDesign,
            Action spawnAction, Action<float> onAmplitudeChanged)
        {
            Assert.IsNotNull(audioSource);
            Assert.IsNotNull(levelDesign);

            _initialized = true;
            _source = audioSource;
            _levelDesign = levelDesign;
            _spawnAction = spawnAction;
            _onAmplitudeChanged = onAmplitudeChanged;

            // make the tiles sync with the music when the tile reaches the middle of the screen.
            _timeToMiddle = Camera.main.orthographicSize / levelDesign.TileSpeed + Constants.MaxAccuracyOffset / 2;

            // webGL.
            _webHandler.Load(levelDesign.Amplitues, levelDesign.Id);
        }

        private void Update()
        {
            if (!_initialized) return;
            _source.GetSpectrumData(_spectrumData, 0, _spectrumWindowType);
            AnalyzeUpdate(_spectrumData);

            // webGL.
            _webHandler.HandleUpdate(_source, _source.time,
                ChangeAmplitude, Spawn, Validate);
        }

        protected abstract void AnalyzeUpdate(float[] spectrumData);
        protected abstract bool Validate(float amplitude);

        protected void Spawn()
        {
            if (Time.time > _lastSpawnTime + _levelDesign.SpawnInterval - _timeToMiddle)
            {
                _spawnAction?.Invoke();
                _lastSpawnTime = Time.time;
            }
        }

        protected void ChangeAmplitude(float amplitude)
        {
            _webHandler.HandleAmplitude(amplitude, _source.time);
            _onAmplitudeChanged.Invoke(amplitude / _source.volume);
        }

        public void Dispose()
        {
            _initialized = false;
            _source = null;
            _spawnAction = null;
            _onAmplitudeChanged = null;
            _timeToMiddle = 0;
        }
    }
}