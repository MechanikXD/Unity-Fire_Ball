using System;
using Core.Game.GameEndArgs;
using Core.Tower;
using UI;
using UI.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Game {
    /// <summary>
    /// Game Manager handles time and state of the game
    /// </summary>
    public class GameManager : MonoBehaviour {
        public static GameManager Instance;
        // Hardcoded since we only have one scene. Should be somewhere in LevelSettings realistically. 
        [Header("Time Related")]
        [SerializeField] private float _timeToComplete;
        [SerializeField] private float _bestTime;
        [SerializeField] private float _goodTime;
        [SerializeField] private float _okTime;
        private float _currentTime;
        
        private bool _gameWasStarted;
        private bool _inGame;

        public bool InGame => _inGame;
        
        public static event Action<GameEndEventArgs> GameEnd;
        /// <summary>
        /// Current remaining time for this level
        /// </summary>
        public float CurrentTime => _currentTime;

        private void OnEnable() => SubscribeToEvents();

        private void Awake() => ToSingleton();

        private void OnDisable() => UnSubscribeFromEvents();
        
        private void Update() {
            if (!_inGame) return;
            // Decrease remaining time
            _currentTime -= Time.deltaTime;
            // Time has ran out
            if (_currentTime > 0) return;

            _inGame = false;
            // Finish game with defeat
            GameEnd?.Invoke(new GameEndEventArgs(_currentTime, GameEndState.Defeat,
                PlayerScore.None));
        }
        /// <summary>
        /// Starts game and countdown to complete the level
        /// </summary>
        public void StartGame() {
            if (_gameWasStarted) return;
            
            _inGame = true;
            _gameWasStarted = true;
            _currentTime = _timeToComplete;
            UIManager.Instance.SetActiveCanvas<HudView>();
        }
        /// <summary>
        /// Finishes the game and invokes GameEnd event.
        /// Calling this function is always results in Victory.
        /// </summary>
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