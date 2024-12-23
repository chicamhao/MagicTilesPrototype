using System;
using System.Collections.Generic;
using UnityEngine;

namespace Apps.Runtime.Domains.WebGL
{
    public sealed class WebGLHandler : MonoBehaviour
    {
        Dictionary<float, float> _amplitudes;
        int _id;

        public void Load(string amplitudes, int id)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            _id = id;
            _amplitudes = AmplitudeSaver.GetDict(amplitudes);
#endif
        }

        public void Save()
        {
            AmplitudeSaver.Save(_id, _amplitudes);
        }

        public void HandleUpdate(
            AudioSource source, float time,
            Action<float> onAmplitudeChanged, Action spawnAction,
            Func<float, bool> validateFunc)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            var key = (float)Math.Round(time, 2);
            if (_amplitudes.TryGetValue(key, out var amplitude))
            {
                onAmplitudeChanged?.Invoke(amplitude);
                if (validateFunc(amplitude))
                {
                    spawnAction.Invoke();
                }
            }
#endif
            if (!source.isPlaying)
            {
                Save();
            }
        }

        public void HandleAmplitude(float amplitude, float time)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            var key = (float)Math.Round(time, 2);
            if (_amplitudes.TryGetValue(key, out var exist))
            {
                if (amplitude > exist)
                {
                    _amplitudes[key] = (float)Math.Round(amplitude, 2);
                }
            }
            else
            {
                _amplitudes[key] = (float)Math.Round(amplitude, 2);
            }
#endif
        }
    }
}