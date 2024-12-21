using Apps.Runtime.Common;
using Apps.Runtime.Interators;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.Runtime.Scenes
{
    public sealed class GameplayScene : MonoBehaviour
    {
        [SerializeField] Gameplay _gameplay;
        [SerializeField] LevelDesign _levelDesign;
        [SerializeField] Button _closeButton;

        private void Awake()
        {
            if (Loader.Instance != null)
            {
                Initialize((int)Loader.Instance.Transition.TransitionData);
            }
            else
            {
                Initialize(0);
            }
            _closeButton.onClick.AddListener(OnClosed);
        }

        private void OnClosed()
        {
            if (Loader.Instance != null)
            {
                Loader.Instance.Transition.Load(Scene.Title);
                return;
            }
            new Transition().Load(Scene.Title);
        }

        // get audio and game stats from data base.
        private void Initialize(int id)
        {
            _gameplay.Initialize(_levelDesign.GetData(id));
        }
    }
}