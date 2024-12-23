using System;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.Runtime.Entities
{
    public sealed class Tile : MonoBehaviour
    {
        public Button Button;
        [SerializeField] RectTransform _rectTransform;

        float _destination; // vertical.
        float _speed;
        Action<Tile> _onDropped;

        public float SpawnTime { get; private set; }
        public float DropDuration { get; private set; }

        public float ClickTime { get; set; }

        private void Awake()
        {
            // bottom out of the screen.
            _destination = -Camera.main.orthographicSize * 2;
        }

        public void Drop(RectTransform lineRect, float speed, Action<Tile> onDropped)
        {
            // translate to the line
            _rectTransform.anchoredPosition = lineRect.localPosition;

            // resize to match the width of the line
            _rectTransform.sizeDelta = new Vector2(lineRect.sizeDelta.x, _rectTransform.sizeDelta.y);

            _destination = -Camera.main.orthographicSize * 2 + transform.position.y;
            _speed = speed;
            _onDropped = onDropped;
            DropDuration = Camera.main.orthographicSize * 2 / _speed;
            SpawnTime = Time.time;
        }

        private void FixedUpdate()
        {
            if (transform.position.y > _destination)
            {
                transform.Translate(_speed * Time.fixedDeltaTime * Vector3.down);
                return;
            }
            _onDropped?.Invoke(this);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
