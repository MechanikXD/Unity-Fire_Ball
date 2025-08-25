using Core.Game;
using Core.Game.GameEndArgs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View {
    public class GameOverView : CanvasBase {
        [SerializeField] private TMP_Text _screenTitle;
        [SerializeField] private Image[] _stars;

        private void OnEnable() {
            SubscribeToEvents();
        }

        protected override void Awake() {
            base.Awake();
            UnloadEndScreen();
        }

        private void OnDisable() {
            UnSubscribeFromEvents();
        }

        private void SubscribeToEvents() {
            GameManager.GameEnd += LoadEndScreen;
        }
        
        private void UnSubscribeFromEvents() {
            GameManager.GameEnd -= LoadEndScreen;
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
    }
}