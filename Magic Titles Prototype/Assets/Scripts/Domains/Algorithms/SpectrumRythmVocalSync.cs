using System;
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

        protected override void AnalyzeUpdate(float[] spectrumData)
        {
            var rhythmAmplitude = 0f; // low frequencies for rhythm
            var vocalAmplitude = 0f; // high frequencies for vocals

            // analyze spectrum
            for (var i = 0; i < spectrumData.Length; i++)
            {
                var frequency = i * AudioSettings.outputSampleRate / 2f / spectrumData.Length;

                if (frequency < _rhythmFrequency)
                {
                    rhythmAmplitude += spectrumData[i];
                }
                else if (frequency >= _vocalFrequency)
                {
                    vocalAmplitude += spectrumData[i];
                }
            }
            _onAmplitudeChanged?.Invoke(Mathf.Max(rhythmAmplitude, vocalAmplitude));

            // detect vocals
            if (vocalAmplitude > _vocalThreshold)
            {
                Spawn();

            }
            // detect rhythms
            else if (rhythmAmplitude > _rhythmThreshold)
            {
                Spawn();
            }
        }
    }
}