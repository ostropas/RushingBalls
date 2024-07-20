using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private SpriteRenderer _graphics;
        private int _count;
        
        public void Init(int count, Color color)
        {
            _graphics.color = color;
            _countText.text = count.ToString();
            _count = count;
        }
        
        public void HitByBall(int damage)
        {
            _count -= damage;
            if (_count <= 0)
            {
                gameObject.SetActive(false);
            }
            else
            {
                _countText.text = _count.ToString();
            }
        }
    }
}
