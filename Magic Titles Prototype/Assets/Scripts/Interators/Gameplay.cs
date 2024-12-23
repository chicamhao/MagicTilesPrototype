using Apps.Runtime.Common;
using Apps.Runtime.Controls;
using Apps.Runtime.Entities;
using Apps.Runtime.Presentations;
using Apps.Runtime.Views;
using TMPro;
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
        [SerializeField] SyncProvider _syncProvider;

        LevelDesignData _levelDesign;
        TileInputHandler _tileInputHandler;
        PointPresenter _pointPresenter;
        AudioSource _source;
        
        public void Initialize(LevelDesignData levelDesign, AudioSource source)
        {
            Assert.IsNotNull(levelDesign);
            Assert.IsNotNull(source);

            // acquire the selected music.
            _levelDesign = levelDesign;
            _source = source;

            // get the synchronize algorithm.
            _syncProvider.Provide(levelDesign.SyncType)
                // synchronize title spawning with the music.
                .Initialize(source, _levelDesign,
                    SpawnTile, _amplitudeVisualizer.Animate);

            // input system.
            _tileInputHandler = new TileInputHandler(OnScoredPoint, OnMissedTile);

            // scoring point system.
            _pointPresenter = new PointPresenter(_pointView);

            _amplitudeVisualizer.MaxAmplitude = _levelDesign.MaxAmplitude;
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

        private void Update()
        {
            if (!_source.isPlaying)
            {
                _source.Stop();
                _pointPresenter.ShowResult(_source.Play);
            }
        }

        private void OnEnable()
        {
            // for debugging, possible to change params at runtime when re-enabling this component.
            //Initialize(FindObjectOfType<AudioSource>(), _levelDesign);
        }
    }
}