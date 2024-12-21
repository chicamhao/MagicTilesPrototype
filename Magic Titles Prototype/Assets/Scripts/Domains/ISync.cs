using System;
using Apps.Runtime.Common;
using UnityEngine;

namespace Apps.Runtime.Domains
{
    public interface ISync : IDisposable
    {
        /// <summary>
        /// synchronize title spawning with the background music.
        /// </summary>
        /// <param name="source">music source to analysis.</param>
        /// <param name="levelDesign">game stats.</param>
        /// <param name="spawnAction">execute spawning tile.</param>
        /// <param name="onAmplitudeChanged">amplitude changed callback.</param>
        public void Initialize(
            AudioSource source,
            LevelDesignData levelDesign,
            Action spawnAction,
            Action<float> onAmplitudeChanged
            );
    }
}