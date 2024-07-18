using System.Collections.Generic;
using Controllers;
using Data;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerData _playerData;
        private GamefieldInputController _gamefieldInputController;
        private AimLineController _aimLineController = new ();
        
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private GameObject _dotPrefab;
        
        private Camera _mainCamera;
        private List<Ball> _instantiatedBalls = new();
        private int _pivotBallIndex;
        private GameplayState _currentState;
        private bool _isAiming;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void Init(PlayerData playerData, Vector2 startPos, GamefieldInputController inputController)
        {
            _gamefieldInputController = inputController;
            _playerData = playerData;
            _pivotBallIndex = 0;
            Vector3 startPos3d = new Vector3(startPos.x, startPos.y, 0);
            GameObject ballsParent = new GameObject
            {
                name = "BallsParent"
            };
            ballsParent.transform.localPosition = Vector3.zero;
            for (int i = 0; i < 50 + playerData.AdditionalBallsCount; i++)
            {
                Ball ball = Instantiate(_ballPrefab, ballsParent.transform);
                ball.transform.position = startPos3d;
                _instantiatedBalls.Add(ball);
            }
            _aimLineController.Init(_dotPrefab, _ballPrefab, ballsParent);

            ChangeState(GameplayState.Aiming);
            _gamefieldInputController.StartAim += StartAim;
            _gamefieldInputController.StopAim += StopAim;
            _gamefieldInputController.ExitAim += ExitAim;
        }

        private void StartAim()
        {
            _isAiming = true;
        }

        private void StopAim()
        {
            _isAiming = false;
            _aimLineController.ClearLine();
            //Shoot
        }

        private void ExitAim()
        {
            _isAiming = false;
            _aimLineController.ClearLine();
        }

        private void ChangeState(GameplayState state)
        {
            _currentState = state;
            switch (state)
            {
               case GameplayState.Aiming:
                    _gamefieldInputController.SetActive(true);
                   break;
               case GameplayState.Moving:
               case GameplayState.Waiting:
                   _gamefieldInputController.SetActive(false);
                   break;
            }
        }

        private Vector2 CalcDirection()
        {
            Ball targetBall = _instantiatedBalls[_pivotBallIndex];
            Vector2 targetBallScreenPos = _mainCamera.WorldToScreenPoint(targetBall.transform.position);
            return new Vector2(Input.mousePosition.x, Input.mousePosition.y) - targetBallScreenPos;
        }

        private void Update()
        {
            if (_currentState == GameplayState.Aiming)
            {
                if (_isAiming)
                {
                    Vector2 dir = CalcDirection();
                    _aimLineController.DrawLine(_instantiatedBalls[_pivotBallIndex].transform.position, dir.normalized); 
                }
            }
        }

        private enum GameplayState
        {
            Waiting,
            Aiming,
            Moving,
        }
    }
}
