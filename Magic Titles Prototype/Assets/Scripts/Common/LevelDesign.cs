using System;
using System.Collections.Generic;
using UnityEngine;

namespace Apps.Runtime.Common
{
    [CreateAssetMenu(fileName = "LevelDesign", menuName = "ScriptableObjects/LevelDesign", order = 1)]
    public sealed class LevelDesign : ScriptableObject
    {
        // notes: it could be a dictionary instead.
        public LevelDesignData GetData(int id) => _data[id];

        [SerializeField] List<LevelDesignData> _data;
    }

    [Serializable]
    public sealed class LevelDesignData
    {
        public AudioClip AudioClip => _audioClip;
        [SerializeField] AudioClip _audioClip;

        [SerializeField] byte _BPM;

        public SyncType SyncType => _syncType;
        [SerializeField] SyncType _syncType;

        public float TileSpeed => _tileSpeed;
        [Range(3f, 10f)]
        [SerializeField] float _tileSpeed;

        public float SpawnInterval => _spawnInterval;
        [Range(1f, 3f)]
        [SerializeField] float _spawnInterval;
    }
}