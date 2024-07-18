using Controllers;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GamefieldController : MonoBehaviour
    {
        private PlayerDataController _playerDataController;
        
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private ObstaclesManager _obstaclesManager;
        [SerializeField] private GamefieldInputController _gamefieldInputController;

        [SerializeField] private Vector2 _ballStartPosition;

        public void Init(PlayerDataController playerDataController)
        {
            _playerDataController = playerDataController;
        }

        private void Start()
        {
            _obstaclesManager.LoadLevel(_playerDataController.PlayerData.CurrentLevel);
            _playerController.Init(_playerDataController.PlayerData, _ballStartPosition, _gamefieldInputController);
        }
        
        public class GamefieldFactory : IFactory<GamefieldController>
        {
            private PlayerDataController _playerDataController;

            public GamefieldFactory(PlayerDataController playerDataController)
            {
                _playerDataController = playerDataController;
            }
            
            public GamefieldController Create()
            {
                GamefieldController prefab = Resources.Load<GamefieldController>("Prefabs/Gamefield");
                GamefieldController instance = Instantiate(prefab);
                instance.Init(_playerDataController);
                return instance;
            }
        }
    }
}
