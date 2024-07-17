using UnityEngine;

namespace Gameplay
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private float _speed;
        public Vector2 Velocity;
    
        private void Start()
        {
            Velocity = Velocity.normalized * _speed;
            _rigidbody2D.AddForce(Velocity, ForceMode2D.Force);
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
            Velocity = Vector3.Reflect(Velocity, contact.normal);
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(Velocity, ForceMode2D.Force);
        }

        private void CompleteBallRound()
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
    }
}
