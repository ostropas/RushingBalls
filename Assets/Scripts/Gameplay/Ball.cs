using System;
using UnityEngine;

namespace Gameplay
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed;
        
        private Vector2 _velocity;
        private Action<Ball> _onEndMoving;

        public void StartMoving(Vector2 velocity, Action<Ball> onEnd)
        {
            _velocity = velocity.normalized * _speed;
            _rigidbody2D.AddForce(_velocity, ForceMode2D.Force);
            _onEndMoving = onEnd;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Border"))
            {
                BounceFromWall(other);
            }
            else if (other.gameObject.CompareTag("BottomBorder"))
            {
                CompleteBallRound();
            }
        }

        private void BounceFromWall(Collision2D collision2D)
        {
            ContactPoint2D contact = collision2D.GetContact(0);
            _velocity = Vector3.Reflect(_velocity, contact.normal);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(_velocity, ForceMode2D.Force);
        }

        private void CompleteBallRound()
        {
            _velocity = Vector2.zero;
            _rigidbody2D.velocity = Vector2.zero;
            _onEndMoving?.Invoke(this);
            _onEndMoving = null;
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
