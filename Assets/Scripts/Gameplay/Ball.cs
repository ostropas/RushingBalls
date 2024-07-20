using System;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed;
        
        public Action<Ball> OnBottomTouched;

        public BallState State { get; private set; }

        private void Start()
        {
            _rigidbody2D.simulated = false;
        }

        public void StartMoving(Vector2 velocity)
        {
            State = BallState.Moving;
            velocity = velocity.normalized * _speed;
            _rigidbody2D.simulated = true;
            _rigidbody2D.AddForce(velocity, ForceMode2D.Force);
            _prevCheckTime = Time.fixedTime;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (State != BallState.Moving) return;
            if (other.gameObject.CompareTag("Obstacle"))
            {
                if (other.gameObject.TryGetComponent(out Obstacle obstacle))
                {
                    obstacle.HitByBall(1);
                }
            }
            else if (other.gameObject.CompareTag("BottomBorder"))
            {
                CompleteBallRound();
            }
        }

        private void CompleteBallRound()
        {
            _rigidbody2D.simulated = false;
            _rigidbody2D.velocity = Vector2.zero;
            OnBottomTouched?.Invoke(this);
        }

        public void SetComebackPoint(Vector2 pos, Action onComplete)
        {
            State = BallState.ComingBack;
            StartCoroutine(MoveToPosCoroutine(pos, onComplete));
        }

        // Check a ball is not stuck in an infinite loop
        private float _prevCheckTime = 0;
        private Vector2 _prevVelocity = Vector2.zero;
        private void FixedUpdate()
        {
            if (Time.fixedTime - _prevCheckTime > 5 && State == BallState.Moving)
            {
                float dot = Vector2.Dot(_rigidbody2D.velocity, _prevVelocity);
                if (1 - Mathf.Abs(dot) < 0.0001f)
                {
                   _rigidbody2D.AddForce(Vector2.down * 0.001f, ForceMode2D.Force); 
                }

                _prevVelocity = _rigidbody2D.velocity;
                _prevCheckTime = Time.fixedTime;
            }
        }

        private IEnumerator MoveToPosCoroutine(Vector2 pos, Action onComplete)
        {
            Vector2 startPos = transform.position;
            float moveTime = 0.2f;
            float t = 0;
            while (t < moveTime)
            {
                float normalizedVal = t / moveTime;
                Vector2 currentPos = Vector2.Lerp(startPos, pos, normalizedVal);
                transform.position = currentPos;
                yield return null;
                t += Time.deltaTime;
            }

            transform.position = pos;
            State = BallState.Waiting;
            onComplete?.Invoke();
        }

        public void RemovePhysics()
        {
            _rigidbody2D.simulated = false;
        }
    }

    public enum BallState
    {
        Waiting,
        Moving,
        ComingBack
    }
}
