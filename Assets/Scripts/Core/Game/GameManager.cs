using System;
using Core.Game.GameEndArgs;
using Core.Tower;
using UI;
using UI.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Game {
    public class GameManager : MonoBehaviour {
        [SerializeField] private float _timeToComplete;
        private float _currentTime;
        private bool _gameWasStarted;
        private bool _inGame;
        [SerializeField] private float _bestTime;
        [SerializeField] private float _goodTime;
        [SerializeField] private float _okTime;
        public static GameManager Instance;

        public bool InGame => _inGame;

        public static event Action<GameEndEventArgs> GameEnd;

        public float CurrentTime => _currentTime;

        private void OnEnable() => SubscribeToEvents();

        private void Awake() => ToSingleton();

        private void OnDisable() => UnSubscribeFromEvents();
        
        private void Update() {
            if (!_inGame) return;

            _currentTime -= Time.deltaTime;

            if (_currentTime > 0) return;

            _inGame = false;

            GameEnd?.Invoke(new GameEndEventArgs(_currentTime, GameEndState.Defeat,
                PlayerScore.None));
        }

        public void StartGame() {
            if (_gameWasStarted) return;
            
            _inGame = true;
            _gameWasStarted = true;
            _currentTime = _timeToComplete;
            UIManager.Instance.SetActiveCanvas<HudView>();
        }

        private void FinishGame(TowerElementDestroyedEventArgs _) {
            _inGame = false;

            var currentTime = _currentTime;
            var gameState = GameEndState.Victory;
            var playerScore = PlayerScore.None;
            
            if (currentTime >= _bestTime) {
                playerScore = PlayerScore.Best;
            }
            else if (currentTime >= _goodTime) {
                playerScore = PlayerScore.Good;
            }
            else if (currentTime >= _okTime) {
                playerScore = PlayerScore.Ok;
            }

            GameEnd?.Invoke(new GameEndEventArgs(currentTime, gameState, playerScore));
        }

        private void ToSingleton() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void SubscribeToEvents() {
            TowerElement.FinalTowerElementDestroyed += FinishGame;
        }

        private void UnSubscribeFromEvents() {
            TowerElement.FinalTowerElementDestroyed -= FinishGame;
        }

        public void RestartScene() {
            _gameWasStarted = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public static void ExitApplication() => Application.Quit();
    }
}