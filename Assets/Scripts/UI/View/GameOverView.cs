﻿using Core.Game;
using Core.Game.GameEndArgs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View {
    public class GameOverView : CanvasBase {
        [SerializeField] private TMP_Text _screenTitle;
        [SerializeField] private Image[] _stars;

        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private void OnEnable() => SubscribeToEvents();

        protected override void Awake() {
            base.Awake();
            UnloadEndScreen();
        }

        private void OnDisable() => UnSubscribeFromEvents();

        private void SubscribeToEvents() {
            GameManager.GameEnd += LoadEndScreen;
            _restartButton.onClick.AddListener(GameManager.Instance.RestartScene);
            _exitButton.onClick.AddListener(GameManager.ExitApplication);
        }
        
        private void UnSubscribeFromEvents() {
            GameManager.GameEnd -= LoadEndScreen;
            _restartButton.onClick.RemoveListener(GameManager.Instance.RestartScene);
            _exitButton.onClick.RemoveListener(GameManager.ExitApplication);
        }
        
        private void UnloadEndScreen() {
            foreach (var star in _stars) {
                star.gameObject.SetActive(false);
            }

            _screenTitle.SetText(string.Empty);
        }

        private void LoadEndScreen(GameEndEventArgs eventArgs) {
            _screenTitle.SetText(eventArgs.GameState.ToString());

            var starCount = (int)eventArgs.Score;
            for (var i = 0; i < starCount; i++) {
                _stars[i].gameObject.SetActive(true);
            }
        }

        public override void EnableCanvas() {
            base.EnableCanvas();
            Time.timeScale = 0f;
        }

        public override void DisableCanvas() {
            base.DisableCanvas();
            Time.timeScale = 1f;
        }
    }
}