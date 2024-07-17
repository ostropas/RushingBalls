using UnityEngine;

namespace Gameplay
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed;
        
        private Vector2 _velocity;

        public void StartMoving(Vector2 velocity)
        {
            _velocity = velocity.normalized * _speed;
            _rigidbody2D.AddForce(_velocity, ForceMode2D.Force);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Border"))
            {
                BounceFromWall(other);
            }
            else if (other.gameObject.CompareTag("BottomBorder"))
            {
                BounceFromWall(other);
                //CompleteBallRound();    
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
        }
    }

    public enum BallState
    {
        Waiting,
        Moving,
        ComingBack
    }
}
