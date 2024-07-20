using System;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private SpriteRenderer _graphics;
        private int _count;
        private Action _onDestroy;
        
        public void Init(int count, Color color, Action onDestroy)
        {
            _graphics.color = color;
            _countText.text = count.ToString();
            _count = count;
            _onDestroy = onDestroy;
        }
        
        public void HitByBall(int damage)
        {
            _count -= damage;
            if (_count <= 0)
            {
                _onDestroy?.Invoke();
                gameObject.SetActive(false);
            }
            else
            {
                _countText.text = _count.ToString();
            }
        }
    }
}
