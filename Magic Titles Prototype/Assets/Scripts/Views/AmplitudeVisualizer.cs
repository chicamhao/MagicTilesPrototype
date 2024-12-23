using UnityEngine;

namespace Apps.Runtime.Views
{
    // for dynamic background.
    public sealed class AmplitudeVisualizer : MonoBehaviour
    {
        [SerializeField] Transform _decorateObject;
        [SerializeField] float _maxScale = 1.5f;
        [SerializeField] float _lerpSpeed = 5f;
        [SerializeField] AnimationCurve _scaleCurve;

        public float MaxAmplitude;

        public void Animate(float amplitude)
        {
            // normalize amplitude to a range of 0 to 1
            var normalizedAmplitude = Mathf.Clamp01(amplitude / MaxAmplitude);

            // evaluate scale based on the animation curve
            var scaleMultiplier = Mathf.Lerp(1, _maxScale, _scaleCurve.Evaluate(normalizedAmplitude));

            // smoothly interpolate to the target scale
            Vector3 targetScale = Vector3.one * scaleMultiplier;
            _decorateObject.localScale = Vector3.Lerp(_decorateObject.localScale, targetScale, Time.deltaTime * _lerpSpeed);
        }
    }
}