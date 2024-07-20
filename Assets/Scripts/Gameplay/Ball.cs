using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameplay
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed;
        
        [SerializeField] private Vector2 _velocity;
        private Vector2 _prevVelocity;
        public Action<Ball> OnBottomTouched;

        public BallState State;

        public void StartMoving(Vector2 velocity)
        {
            State = BallState.Moving;
            _velocity = velocity.normalized * _speed;
            _rigidbody2D.AddForce(_velocity, ForceMode2D.Force);
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

        /*
        private void BounceFromWall(Collision2D collision2D)
        {
            ContactPoint2D contact = collision2D.GetContact(0);
            _velocity = Vector3.Reflect(_velocity, contact.normal);
            if (1 - Vector2.Dot(_velocity, _prevVelocity) < 0.001f)
            {
                _velocity = Quaternion.AngleAxis(Random.Range(-1, 1), Vector3.forward) * _velocity;
            }
            _prevVelocity = _velocity;
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(_velocity, ForceMode2D.Force);
        }

        private void FixedUpdate()
        {
            if (State == BallState.Moving)
            {
                if (_rigidbody2D.velocity.magnitude < _velocity.magnitude * 0.95f)
                {
                    _rigidbody2D.velocity = Vector2.zero;
                    _rigidbody2D.AddForce(_velocity, ForceMode2D.Force);
                }
            }
        }
        */

        private void CompleteBallRound()
        {
            _velocity = Vector2.zero;
            _rigidbody2D.velocity = Vector2.zero;
            OnBottomTouched?.Invoke(this);
        }

        public void SetComebackPoint(Vector2 pos, Action onComplete)
        {
            State = BallState.ComingBack;
            StartCoroutine(MoveToPosCoroutine(pos, onComplete));
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
