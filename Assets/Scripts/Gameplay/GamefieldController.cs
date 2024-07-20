using Controllers;
using Data;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GamefieldController : MonoBehaviour
    {
        private PlayerDataController _playerDataController;
        private LevelsStorage _levelsStorage;
        
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
            _playerController.Init(_playerDataController.PlayerData, _ballStartPosition, _gamefieldInputController);
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
