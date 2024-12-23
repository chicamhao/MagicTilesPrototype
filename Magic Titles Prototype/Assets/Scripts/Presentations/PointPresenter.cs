using System;
using Apps.Runtime.Common;
using Apps.Runtime.Domains;
using Apps.Runtime.Views;
using UnityEngine.Assertions;

namespace Apps.Runtime.Presentations
{
    public sealed class PointPresenter
    {
        readonly PointView _view;
        uint _currentPoint;

        public PointPresenter(PointView view) : this(view, 0) { }

        public PointPresenter(PointView view, uint point)
        {
            Assert.IsNotNull(view);
            _view = view;
            _currentPoint = point;
            _view.SetValue(PointRank.None, point, point);
        }

        public void ScorePoint(float spawnTime, float dropDuration, float clickTime)
        {
            var (rank, point) = PointCalculator.CalculatePoint(spawnTime, dropDuration, clickTime);

            var previousPoint = _currentPoint;
            _currentPoint += point;
            _view.SetValue(rank, previousPoint, _currentPoint);
        }

        public void ShowResult(Action onRetried)
        {
            _view.RetryButton.gameObject.SetActive(true);
            _view.PointText.alpha = 1;

            _view.RetryButton.onClick.AddListener(() =>
            {
                onRetried?.Invoke();
                _view.RetryButton.gameObject.SetActive(false);
                _view.PointText.alpha = 0;
            });
        }
    }
}