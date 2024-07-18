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
        private PlayerData _playerData;
        private GamefieldInputController _gamefieldInputController;
        private AimLineController _aimLineController = new ();
        
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private GameObject _dotPrefab;
        [SerializeField] private float _delayBetweenShots;
        [SerializeField] private float _angleClamp = 80;
        
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

        private void Shoot(Vector2 dir)
        {
            ChangeState(GameplayState.Moving);
            StartCoroutine(ShootCoroutine(dir));
        }

        private IEnumerator ShootCoroutine(Vector2 dir)
        {
            WaitForSeconds delay = new WaitForSeconds(_delayBetweenShots);
            int ballsReturned = 0;
            int firstBall = -1;
            foreach (Ball instantiatedBall in _instantiatedBalls)
            {
               instantiatedBall.StartMoving(dir, ball =>
               {
                   if (firstBall == -1)
                   {
                       firstBall = _instantiatedBalls.IndexOf(ball);
                       ballsReturned++;
                   }
                   else
                   {
                       ball.StartCoroutine(MoveToFirstCoroutine(ball, _instantiatedBalls[firstBall].transform.position,
                           () =>
                           {
                               ballsReturned++;
                           }));
                   }
               });
               yield return delay;
            }

            while (ballsReturned != _instantiatedBalls.Count)
            {
                yield return null;
            }
            
            ChangeState(GameplayState.Aiming);
        }

        private IEnumerator MoveToFirstCoroutine(Ball ball, Vector2 pos, Action onComplete)
        {
            Vector2 startPos = ball.transform.position;
            float moveTime = 0.2f * (startPos - pos).magnitude;
            if (moveTime > 0.01)
            {
                float t = 0;
                while (t < moveTime)
                {
                    float normalizedVal = t / moveTime;
                    Vector2 currentPos = Vector2.Lerp(startPos, pos, normalizedVal);
                    ball.transform.position = currentPos;
                    yield return null;
                    t += Time.deltaTime;
                }
            }

            ball.transform.position = pos;
            onComplete?.Invoke();
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
            if (_currentState == GameplayState.Aiming)
            {
                if (_isAiming)
                {
                    Vector2 dir = ClampRot(CalcDirection(), _angleClamp);
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
