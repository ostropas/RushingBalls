using System;
using System.Collections;
using Controllers;
using Data;
using UI;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GamefieldController : MonoBehaviour
    {
        public event Action ScoreUpdated; 
        
        public float LevelProgress => _obstaclesManager.LevelProgress;
        public int Score { get; private set; }

        private PlayerDataController _playerDataController;
        private LevelsStorage _levelsStorage;
        private ViewManager _viewManager;
        
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private ObstaclesManager _obstaclesManager;
        [SerializeField] private GamefieldInputController _gamefieldInputController;
        [SerializeField] private int MaxScoreMultiplier;

        [SerializeField] private Vector2 _ballStartPosition;

        [Inject]
        public void Init(PlayerDataController playerDataController, LevelsStorage levelsStorage, ViewManager viewManager)
        {
            _playerDataController = playerDataController;
            _levelsStorage = levelsStorage;
            _viewManager = viewManager;
        }

        private void Start()
        {
            _obstaclesManager.LoadLevel(_playerDataController.PlayerData.CurrentLevel, _levelsStorage);
            _obstaclesManager.OnObstacleDestroy += OnObstacleDestroy;
            _obstaclesManager.OnLevelCompleted += OnLevelCompleted;
            _playerController.Init(_ballStartPosition, _gamefieldInputController);
        }

        private void OnObstacleDestroy(int score)
        {
            int correctedScore = Mathf.Max(MaxScoreMultiplier - _playerController.NumberOfShoots + 1, 1);
            Score += score * correctedScore;
            ScoreUpdated?.Invoke(); 
        }

        private void OnLevelCompleted()
        {
            StartCoroutine(LevelCompleteCoroutine());
        }

        private IEnumerator LevelCompleteCoroutine()
        {
            yield return new WaitWhile(() => _playerController.State == PlayerController.GameplayState.Moving);
           _playerDataController.LevelCompleted(Score);
           _viewManager.Hide<GameplayPanelController>();
           _viewManager.Show<MultiplierMenuController>();
           Destroy(gameObject);
        }
        
        public class GamefieldFactory : IFactory<GamefieldController>
        {
            private DiContainer _container;

            public GamefieldFactory(DiContainer container)
            {
                _container = container;
            }
            
            public GamefieldController Create()
            {
                GamefieldController prefab = Resources.Load<GamefieldController>("Prefabs/Gamefield/Gamefield");
                GamefieldController instance = _container.InstantiatePrefabForComponent<GamefieldController>(prefab.gameObject);
                return instance;
            }
        }
    }
}
