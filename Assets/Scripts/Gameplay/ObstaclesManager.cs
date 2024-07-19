using UnityEngine;

namespace Gameplay
{
    public class ObstaclesManager : MonoBehaviour
    {
        [SerializeField] private GameObject _first;
        [SerializeField] private float _count;
        [SerializeField] private float _space;
        
        public void LoadLevel(int levelIndex) {
            
        }

        [ContextMenu("Generate")]
        private void Generate()
        {
            foreach (Transform o in transform)
            {
                if (o != _first.transform)
                {
                   DestroyImmediate(_first); 
                }
            }
            

            float prevPos = _first.transform.position.x;
            for (int i = 0; i < _count; i++)
            {
                prevPos += _space;
                var obj = Instantiate(_first, _first.transform.parent);
                Vector3 pos = _first.transform.position;
                pos.x = prevPos;
                obj.transform.position = pos;
            }
        }
    }
}
