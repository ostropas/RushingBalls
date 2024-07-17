using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class GamefieldController : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private ObstaclesManager _obstaclesManager;

        [SerializeField] private Vector2 _ballStartPosition;

        private void Start()
        {
            _playerController.Init(_ballStartPosition);
        }
        
        public class GamefieldFactory : IFactory<GamefieldController>
        {
            public GamefieldController Create()
            {
                GamefieldController prefab = Resources.Load<GamefieldController>("Prefabs/Gamefield");
                return Instantiate(prefab);
            }
        }
    }
}
