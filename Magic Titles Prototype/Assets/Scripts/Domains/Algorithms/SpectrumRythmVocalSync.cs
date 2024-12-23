using System;
using TMPro;
using UnityEngine;

namespace Apps.Runtime.Domains.Algorithms
{
    /// <summary>
    /// detect vocals & rhythms based on the high frequencies of spectrum data.
    /// priority policy: vocal > rhythm
    /// TODO makes it possible to spawn 2 tiles at the same time.
    /// </summary>
    public sealed class SpectrumRythmVocalSync : SpectrumSync
    {
        [SerializeField] float _rhythmThreshold = 0.3f;
        [SerializeField] float _vocalThreshold = 1f;

        [Header("Number of sound wave cycles that occur in 1 second (Hz).")]
        [Range(20f, 250f)]
        [SerializeField] float _rhythmFrequency = 200f;

        [Range(250f, 3000f)]
        [SerializeField] float _vocalFrequency = 1000f;

        float _rhythmAmplitude; // low frequencies for rhythm
        float _vocalAmplitude; // high frequencies for vocals

        protected override void AnalyzeUpdate(float[] spectrumData)
        {
            // analyze spectrum
            _rhythmAmplitude = 0f;
            _vocalAmplitude = 0f;
            for (var i = 0; i < spectrumData.Length; i++)
            {
                var frequency = i * AudioSettings.outputSampleRate / 2f / spectrumData.Length;

                if (frequency < _rhythmFrequency * _volumn)
                {
                    _rhythmAmplitude += spectrumData[i];
                }
                else if (frequency >= _vocalFrequency * _volumn)
                {
                    _vocalAmplitude += spectrumData[i];
                }
            }
            ChangeAmplitude(Mathf.Max(_rhythmAmplitude, _vocalAmplitude));

            // detect vocals
            if (_vocalAmplitude > _vocalThreshold * _volumn)
            {
                Spawn();
            }
            // detect rhythms
            else if (_rhythmAmplitude > _rhythmThreshold * _volumn)
            {
                Spawn();
            }
        }

        protected override bool Validate(float amplitude)
        {
            return amplitude > _vocalThreshold * _volumn || amplitude > _rhythmThreshold * _volumn;
        }
    }
}