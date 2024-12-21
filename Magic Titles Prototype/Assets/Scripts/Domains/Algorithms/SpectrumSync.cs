using System;
using Apps.Runtime.Common;
using UnityEngine;
using UnityEngine.Assertions;

namespace Apps.Runtime.Domains.Algorithms
{
    public abstract class SpectrumSync : MonoBehaviour, ISync
    {
        [SerializeField] FFTWindow _spectrumWindowType = FFTWindow.Rectangular;
        readonly float[] _spectrumData = new float[512]; // note: could be a performance impact?

        AudioSource _source;
        LevelDesignData _levelDesign;
        Action _spawnAction;

        bool _initialized;
        float _timeToMiddle;
        float _lastSpawnTime;
        protected Action<float> _onAmplitudeChanged;

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
        }

        private void Update()
        {
            if (_initialized && Time.time > _lastSpawnTime + _levelDesign.SpawnInterval - _timeToMiddle)
            {
                _source.GetSpectrumData(_spectrumData, 0, _spectrumWindowType);
                AnalyzeUpdate(_spectrumData);
            }
        }

        protected abstract void AnalyzeUpdate(float[] spectrumData);

        protected void Spawn()
        {
            _spawnAction?.Invoke();
            _lastSpawnTime = Time.time;
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