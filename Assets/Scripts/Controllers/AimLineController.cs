using System.Collections.Generic;
using Gameplay;
using UnityEngine;

namespace Controllers
{
    public class AimLineController
    {
        private const float _distanceBetweenDots = 0.3f;
        private const float _reflectSize = 2f;
        private GameObject _dotPrefab;
        private GameObject _parent;
        
        private List<GameObject> _dots = new();
        private GameObject _ball;
        private Vector3 _prevDirection;
        
        public void Init(GameObject dotPrefab, Ball ballPrefab, GameObject parent)
        {
            _dotPrefab = dotPrefab;
            _parent = parent;

            Ball ball = Object.Instantiate(ballPrefab, _parent.transform);
            ball.RemovePhysics();
            _ball = ball.gameObject;
            _ball.SetActive(false);
        }

        public void DrawLine(Vector2 start, Vector2 direction)
        {
            float dotVal = Vector2.Dot(_prevDirection, direction);
            if ((1 - dotVal) < 0.00001f) return;
            _prevDirection = direction;
            Debug.Log("Draw");
            ClearLine();

            int i = 0;

            int mask = LayerMask.GetMask("BallObstacle");

            RaycastHit2D hit = Physics2D.Raycast(start, direction, Mathf.Infinity, mask);

            DrawLine(start, hit.point, ref i);

            _ball.transform.position = hit.point;
            _ball.SetActive(true);

            Vector2 reflect = Vector2.Reflect(direction, hit.normal);
            reflect = reflect.normalized * _reflectSize;
            DrawLine(hit.point, hit.point + reflect, ref i);
        }

        private void DrawLine(Vector2 start, Vector2 end, ref int dotIndex)
        {
            float step = _distanceBetweenDots / (end - start).magnitude;
            float t = 0;
            while (t < 1)
            {
                Vector2 pos = Vector2.Lerp(start, end, t);
                GameObject dot = GetDot(dotIndex++);
                dot.transform.position = pos;
                t += step;
            }
        }

        private GameObject GetDot(int index)
        {
            if (index < _dots.Count)
            {
                _dots[index].SetActive(true);
                return _dots[index];
            }
            else
            {
                GameObject dot = Object.Instantiate(_dotPrefab, _parent.transform);
                _dots.Add(dot);
                return dot;
            }
        }

        public void ClearLine()
        {
            foreach (var dot in _dots)
            {
               dot.SetActive(false); 
            } 
            _ball.SetActive(false);
        }
    }
}