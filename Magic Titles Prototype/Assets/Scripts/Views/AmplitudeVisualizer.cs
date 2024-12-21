using UnityEngine;

namespace Apps.Runtime.Views
{
    // for dynamic background.
    public sealed class AmplitudeVisualizer : MonoBehaviour
    {
        [Header("Effect Settings")]
        [SerializeField] Transform _decorateObject;
        [SerializeField] float _maxScale = 1.5f;
        [SerializeField] float _lerpSpeed = 5f;
        [SerializeField] float _multiplier = 1.5f;

        Vector3 originalScale;

        void Start()
        {
            if (_decorateObject != null)
            {
                originalScale = _decorateObject.localScale;
            }
        }

        public void Animate(float amplitude)
        {
            // calculate target scale.
            Vector3 targetScale = originalScale * Mathf.Clamp(1f + amplitude * _multiplier, 1f, _maxScale);

            // smoothly interpolate to the target scale.
            _decorateObject.localScale = Vector3.Lerp(_decorateObject.localScale, targetScale, Time.deltaTime * _lerpSpeed);
        }
    }
}