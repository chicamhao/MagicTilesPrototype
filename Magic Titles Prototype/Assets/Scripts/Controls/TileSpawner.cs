using Apps.Runtime.Entities;
using UnityEngine;
using UnityEngine.Pool;

namespace Apps.Runtime.Controls
{
    public sealed class TileSpawner : MonoBehaviour
    {
        [SerializeField] Tile _titlePrefab;
        [SerializeField] int _defaultPoolCapacity = 10;

        IObjectPool<Tile> _objectPool;

        private void Awake()
        {
            _objectPool = new ObjectPool<Tile>(
                createFunc: () => Instantiate(_titlePrefab, transform),
                actionOnGet: (t) => t.SetActive(true),
                actionOnRelease: (t) => t.SetActive(false),
                actionOnDestroy: (t) => Destroy(t),
                defaultCapacity: _defaultPoolCapacity);
        }

        public Tile Get()
        {
            return _objectPool.Get();
        }

        public void Release(Tile tile)
        {
            _objectPool.Release(tile);
        }
    }
}