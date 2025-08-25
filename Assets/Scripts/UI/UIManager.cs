using System;
using System.Collections.Generic;
using Core.Game;
using Core.Game.GameEndArgs;
using UI.View;
using UnityEngine;

namespace UI {
    public class UIManager : MonoBehaviour {
        public static UIManager Instance;
        [SerializeField] private CanvasBase[] _canvases;
        private int _activeCanvasIndex;

        private Dictionary<Type, (int index, CanvasBase canvas)> _canvasByType;

        private void OnEnable() {
            SubscribeToEvents();
        }

        private void Awake() {
            ToSingleton();
            TypeCanvases();
        }

        private void OnDisable() {
            UnSubscribeFromEvents();
        }

        private void Start() {
            DisableAllCanvases();
            SetActiveCanvas<GameStartView>();
        }

        private void SubscribeToEvents() {
            GameManager.GameEnd += ShowGameEndScreen;
        }

        private void ShowGameEndScreen(GameEndEventArgs obj) => SetActiveCanvas<GameOverView>();

        private void UnSubscribeFromEvents() {
            GameManager.GameEnd -= ShowGameEndScreen;
        }

        private void ToSingleton() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void TypeCanvases() {
            _canvasByType = new Dictionary<Type, (int index, CanvasBase canvas)>();
            for (var i = 0; i < _canvases.Length; i++) {
                if (_canvasByType.ContainsKey(_canvases[i].GetType())) {
                    Debug.LogWarning(
                        $"Canvas with the same type already exists: {_canvases[i].GetType()}");
                }
                else {
                    _canvasByType.Add(_canvases[i].GetType(), (i, _canvases[i]));
                }
            }
        }

        private void DisableAllCanvases() {
            foreach (var canvas in _canvases) canvas.DisableCanvas();
            _activeCanvasIndex = -1;
        }

        public void SetActiveCanvas<T>(bool disablePrevious = true) where T : CanvasBase {
            var canvas = GetCanvasWithIndex<T>();
            if (canvas.canvas != null) {
                canvas.canvas.EnableCanvas();
            }

            if (disablePrevious && _activeCanvasIndex != -1) {
                _canvases[_activeCanvasIndex].DisableCanvas();
            }

            _activeCanvasIndex = canvas.index;
        }

        public T GetCanvas<T>() where T : CanvasBase => GetCanvasWithIndex<T>().canvas;

        private (int index, T canvas) GetCanvasWithIndex<T>() where T : CanvasBase {
            if (_canvasByType.ContainsKey(typeof(T)))
                return ((int, T))_canvasByType[typeof(T)];

            Debug.LogWarning($"There are no canvas of type {typeof(T)}");
            return (-1, null);
        }
    }
}