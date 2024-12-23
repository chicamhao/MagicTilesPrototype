using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using Apps.Runtime.Common;

namespace Apps.Runtime.Views
{
    public sealed class PointView : MonoBehaviour
    {
        public Button RetryButton;
        public TMP_Text PointText;

        [Header("Rank.")]
        [SerializeField] Image[] _rankImages;

        [Header("Background decoration.")]
        [SerializeField] float _decorationAlpha = 0.7f;
        [SerializeField] Image[] _decorations;

        [SerializeField] Vector3 _rankScale = new(.2f, .2f, 1f); // current scale
        [SerializeField] Vector3 _newRankScale = new(.25f, .25f, 1f); // target scale

        [SerializeField] float _increaseDuration = 0.3f;
        [SerializeField] float _fadeDuration = 0.15f;
        Coroutine _coroutine;

        private void Start()
        {
            SetAlpha(0f);
        }

        public void SetValue(PointRank rank, uint previousPoint, uint newPoint)
        {
            // rank validation.
            if (rank == PointRank.None)
                return;

            _rankImages[1].gameObject.SetActive(rank == PointRank.Great || rank == PointRank.Perfect);
            _rankImages[2].gameObject.SetActive(rank == PointRank.Perfect);

            // point value validation.
            if (newPoint - previousPoint <= 0)
                return;

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(PlayAnimationAsync(previousPoint, newPoint));
        }

        private IEnumerator PlayAnimationAsync(uint previousPoint, uint newPoint)
        {
            SetAlpha(1f);
            yield return IncreasePointAsync(previousPoint, newPoint);
            yield return ChangeAlphaAsync(1f, 0f, _fadeDuration);
            _coroutine = null;
        }

        private IEnumerator IncreasePointAsync(uint previousPoint, uint newPoint)
        {
            float elapsedTime = 0f;
            while (elapsedTime < _increaseDuration)
            {
                elapsedTime += Time.deltaTime;

                // interpolated point value.
                var t = elapsedTime / _increaseDuration;
                var displayedPoint = Mathf.RoundToInt(Mathf.Lerp(previousPoint, newPoint, t));
                PointText.SetText("{0}", displayedPoint);

                // interpolate scale.
                foreach (var image in _rankImages)
                {
                    image.rectTransform.localScale = Vector3.Lerp(_rankScale, _newRankScale, t);
                }

                yield return null;
            }

            // ensure the final point is displayed.
            PointText.SetText("{0}", newPoint);
            foreach (var image in _rankImages)
            {
                image.rectTransform.localScale = _newRankScale;
            }
        }

        private IEnumerator ChangeAlphaAsync(float startAlpha, float endAlpha, float duration)
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // interpolate alpha.
                var alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                SetAlpha(alpha);

                yield return null;
            }

            // final values.
            SetAlpha(endAlpha);
        }

        private void SetAlpha(float alpha)
        {
            PointText.alpha = alpha;

            foreach (var image in _rankImages)
            {
                var temp = image.color;
                temp.a = alpha;
                image.color = temp;
            }

            // TODO should be implemented separately.
            foreach (var image in _decorations)
            {
                var temp = image.color;
                temp.a = Mathf.Max(_decorationAlpha, alpha);
                image.color = temp;
            }
        }
    }
}