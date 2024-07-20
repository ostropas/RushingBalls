using System;
using Controllers;
using Data;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GamefieldController : MonoBehaviour
    {
        public int MaxScore => _obstaclesManager.MaxScore;
        
        private PlayerDataController _playerDataController;
        private LevelsStorage _levelsStorage;

        private int _totalScore;

        public event Action<int> ScoreUpdated; 
        
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private ObstaclesManager _obstaclesManager;
        [SerializeField] private GamefieldInputController _gamefieldInputController;

        [SerializeField] private Vector2 _ballStartPosition;

        [Inject]
        public void Init(PlayerDataController playerDataController, LevelsStorage levelsStorage)
        {
            _playerDataController = playerDataController;
            _levelsStorage = levelsStorage;
        }

        private void Start()
        {
            _obstaclesManager.LoadLevel(_playerDataController.PlayerData.CurrentLevel, _levelsStorage);
            _obstaclesManager.OnObstacleDestroy += OnObstacleDestroy;
            _playerController.Init(_playerDataController.PlayerData, _ballStartPosition, _gamefieldInputController);
        }

        private void OnObstacleDestroy(int score)
        {
            _totalScore += score;
            ScoreUpdated?.Invoke(_totalScore); 
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
