using UnityEngine;

namespace Apps.Runtime.Domains.Algorithms
{
    /// <summary>
    /// article analysis http://www.rotorbrain.com/foote/papers/icme2001/icmehtml.htm
    /// </summary>
    public sealed class SpectrumBeatSync : SpectrumSync
    {
        [SerializeField] float _amplitudeThreshold;

        protected override void AnalyzeUpdate(float[] spectrumData)
        {
            // detect a beat based on amplitude threshold
            var peakAmplitude = 0f;
            foreach (var amplitude in spectrumData)
            {
                if (amplitude > peakAmplitude)
                {
                    peakAmplitude = amplitude;
                }
            }
            ChangeAmplitude(peakAmplitude);

            if (peakAmplitude > _amplitudeThreshold * _volumn)
            {
                Spawn();
            }
        }

        protected override bool Validate(float amplitude)
        {
            return amplitude > _amplitudeThreshold * _volumn;
        }
    }
}