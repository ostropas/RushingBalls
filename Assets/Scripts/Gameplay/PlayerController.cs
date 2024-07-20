using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Data;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        public int NumberOfShoots { get; private set; }
        public GameplayState State { get; private set; }

        private GamefieldInputController _gamefieldInputController;
        private AimLineController _aimLineController = new ();
        
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private GameObject _dotPrefab;
        [SerializeField] private float _delayBetweenShots;
        [SerializeField] private float _angleClamp = 80;
        
        private Camera _mainCamera;
        private List<Ball> _instantiatedBalls = new();
        private int _pivotBallIndex;
        private Vector2 _startBallsPos;
        private bool _isAiming;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public void Init(Vector2 startPos, GamefieldInputController inputController)
        {
            _gamefieldInputController = inputController;
            _pivotBallIndex = 0;
            _startBallsPos = startPos;
            Vector3 startPos3d = new Vector3(startPos.x, startPos.y, 0);
            GameObject ballsParent = new GameObject
            {
                name = "BallsParent"
            };
            ballsParent.transform.SetParent(transform);
            ballsParent.transform.localPosition = Vector3.zero;
            for (int i = 0; i < 50; i++)
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
            Vector2 dir = ClampRot(CalcDirection(), _angleClamp);
            Shoot(dir);
        }

        private void ExitAim()
        {
            _isAiming = false;
            _aimLineController.ClearLine();
        }

        private void ChangeState(GameplayState state)
        {
            State = state;
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

        private void Shoot(Vector2 dir)
        {
            NumberOfShoots++;
            ChangeState(GameplayState.Moving);
            StartCoroutine(ShootCoroutine(dir));
        }

        private IEnumerator ShootCoroutine(Vector2 dir)
        {
            WaitForSeconds delay = new WaitForSeconds(_delayBetweenShots);
            Ball targetComebackBall = null;
            Vector2 comebackPos = Vector2.zero;
            int ballsReturned = 0;
            
            foreach (var instantiatedBall in _instantiatedBalls)
            {
                instantiatedBall.StartMoving(dir);
                instantiatedBall.OnBottomTouched = ball =>
                {
                    if (targetComebackBall == null)
                    {
                        targetComebackBall = ball;
                        _pivotBallIndex = _instantiatedBalls.IndexOf(ball);
                        comebackPos = targetComebackBall.transform.position;
                        comebackPos.y = _startBallsPos.y;
                    }

                    ball.SetComebackPoint(comebackPos, () => ballsReturned++);
                };
                yield return delay;
            }

            while (ballsReturned != _instantiatedBalls.Count)
            {
                yield return null;
            }
            
            ChangeState(GameplayState.Aiming);
        }

        private Vector2 CalcDirection()
        {
            Ball targetBall = _instantiatedBalls[_pivotBallIndex];
            Vector2 targetBallScreenPos = _mainCamera.WorldToScreenPoint(targetBall.transform.position);
            return new Vector2(Input.mousePosition.x, Input.mousePosition.y) - targetBallScreenPos;
        }

        private Vector2 ClampRot(Vector2 vector2, float angle)
        {
            float vectorAngle = Vector2.Angle(Vector2.up, vector2) * Mathf.Sign(vector2.x);
            float clampedVal = Mathf.Clamp(vectorAngle, -angle, angle);
            return Quaternion.AngleAxis(clampedVal, -Vector3.forward) * Vector3.up;
        }

        private void Update()
        {
            if (State == GameplayState.Aiming)
            {
                if (_isAiming)
                {
                    Vector2 dir = ClampRot(CalcDirection(), _angleClamp);
                    _aimLineController.DrawLine(_instantiatedBalls[_pivotBallIndex].transform.position, dir.normalized); 
                }
            }
        }

        public enum GameplayState
        {
            Waiting,
            Aiming,
            Moving,
        }
    }
}
