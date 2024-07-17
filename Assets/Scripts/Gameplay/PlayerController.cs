using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Ball _ballPrefab;
        private List<Ball> _instantiatedBalls;
        private Vector2 _ballsStartPosition;

        public void Init(Vector2 startPos)
        {
            _ballsStartPosition = startPos; 
        }
    }
}
