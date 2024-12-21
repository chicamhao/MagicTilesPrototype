using System;
using System.Collections.Generic;
using Apps.Runtime.Entities;
using UnityEngine;

namespace Apps.Runtime.Controls
{
    public sealed class TileInputHandler
    {
        readonly Queue<Tile> _tileQueue = new();
        readonly Action<Tile> _onScored;
        readonly Action _onMissed;

        public TileInputHandler(Action<Tile> onScored, Action onMissed)
        {
            _onScored = onScored;
            _onMissed = onMissed;
        }

        public void Handle(Tile tile)
        {
            _tileQueue.Enqueue(tile);
            tile.Button.interactable = true;

            // the first tile
            if (tile == _tileQueue.Peek())
            {
                tile.Button.onClick.AddListener(() => OnClicked(tile));
            }
        }

        private void OnClicked(Tile tile)
        {
            // click a non-peek tile, nothing to do
            if (!TryHandleInternal(tile))
                return;

            tile.ClickTime = Time.time;
            _onScored?.Invoke(tile);
        }

        public void Release(Tile tile)
        {
            // release a clicked tile, nothing to do
            if (!TryHandleInternal(tile))
                return;

            tile.Button.onClick.RemoveAllListeners();
            _onMissed?.Invoke();
        }

        private bool TryHandleInternal(Tile tile)
        {
            // process with peek only
            if (_tileQueue.TryPeek(out var peak))
            {
                if (tile == peak)
                {
                    // release the peak tile
                    _tileQueue.Dequeue();
                    peak.Button.interactable = false;
                    peak.Button.onClick.RemoveAllListeners();

                    // enable the next tile
                    if (_tileQueue.TryPeek(out var next))
                    {
                        next.Button.onClick.AddListener(() => OnClicked(next));
                    }
                    return true;
                }
            }
            return false;
        }
    }
}