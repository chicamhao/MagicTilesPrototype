using Apps.Runtime.Common;
using Apps.Runtime.Controls;
using Apps.Runtime.Entities;
using Apps.Runtime.Presentations;
using Apps.Runtime.Views;
using UnityEngine;
using UnityEngine.Assertions;

namespace Apps.Runtime.Interators
{
    public sealed class Gameplay : MonoBehaviour
    {
        [SerializeField] LineSelector _lineSelector;
        [SerializeField] TileSpawner _tileSpawner;
        [SerializeField] PointView _pointView;
        [SerializeField] AmplitudeVisualizer _amplitudeVisualizer;

        [Header("Audio analysis & synchronous algorithms")]
        [SerializeField] AudioSource _audioSource;
        [SerializeField] SyncProvider _syncProvider;
        [SerializeField] SyncType _syncType;

        LevelDesignData _levelDesign;
        TileInputHandler _tileInputHandler;
        PointPresenter _pointPresenter;

        public void Initialize(LevelDesignData levelDesign)
        {
            Assert.IsNotNull(levelDesign);

            // acquire the selected music.
            _levelDesign = levelDesign;
            _audioSource.clip = levelDesign.AudioClip;
            _audioSource.Play();

            // get the synchronize algorithm.
            _syncProvider.Provide(_syncType)
                // synchronize title spawning with the music.
                .Initialize(_audioSource, _levelDesign,
                    SpawnTile, _amplitudeVisualizer.Animate);

            // input system.
            _tileInputHandler = new TileInputHandler(OnScoredPoint, OnMissedTile);

            // scoring point system.
            _pointPresenter = new PointPresenter(_pointView);
        }

        private void SpawnTile()
        {
            var tile = _tileSpawner.Get();
            _tileInputHandler.Handle(tile);

            tile.Drop(_lineSelector.GetLine(), _levelDesign.TileSpeed, ReleaseTile);
        }

        private void ReleaseTile(Tile tile)
        {
            _tileInputHandler.Release(tile);
            _tileSpawner.Release(tile);
        }

        private void OnScoredPoint(Tile tile)
        {
            _pointPresenter.ScorePoint(tile.SpawnTime, tile.DropDuration, tile.ClickTime);
        }

        private void OnMissedTile()
        {
            // TODO game over?
        }

        private void OnEnable()
        {
            // for debugging, possible to change params at runtime when re-enabling this component.
            //Initialize(FindObjectOfType<AudioSource>(), _levelDesign);
        }
    }
}