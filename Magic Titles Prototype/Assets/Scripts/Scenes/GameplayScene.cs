using System;
using Apps.Runtime.Common;
using Apps.Runtime.Interators;
using UnityEngine;

namespace Apps.Runtime.Scenes
{
    public sealed class GameplayScene : MonoBehaviour
    {
        [SerializeField] Gameplay _gameplay;
        [SerializeField] LevelDesign _levelDesign;

        private void Awake()
        {
            if (Loader.Instance != null)
            {
                Initialize((int)Loader.Instance.Transition.TransitionData);
                return;
            }
            Initialize(0);
        }

        // get audio and game stats from data base.
        private void Initialize(int id)
        {
            _gameplay.Initialize(_levelDesign.GetData(id));
        }
    }
}