using System;
using Apps.Runtime.Common;
using Apps.Runtime.Domains;
using Apps.Runtime.Domains.Algorithms;
using UnityEngine;

namespace Apps.Runtime.Interators
{
    public sealed class SyncProvider : MonoBehaviour
    {
        private ISync _holder;
        private SyncType _type;

        public ISync Provide(SyncType type)
        {
            if (_type == type && _holder != null)
                return _holder;

            _type = type;
            _holder?.Dispose();

            _holder = type switch
            {
                SyncType.SpectrumBeat => GetComponent<SpectrumBeatSync>(),
                SyncType.SpectrumRythmVocal => GetComponent<SpectrumRythmVocalSync>(),
                _ => throw new NotImplementedException(type.ToString())
            };

            return _holder ?? throw new ArgumentNullException(type.ToString());
        }
    }
}