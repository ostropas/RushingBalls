using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public class Pool<T> where T : MonoBehaviour
    {
        private readonly Stack<T> _stack = new();
        private Transform _parent;
        private T _prefab;

        public void Init(T prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public void ReturnElement(T element)
        {
           element.gameObject.SetActive(false);
           _stack.Push(element);
        }

        public T GetElement()
        {
            T res;
            if (_stack.Count == 0)
            {
                res = Object.Instantiate(_prefab, _parent);
            }
            else
            {
                res = _stack.Pop();
                res.gameObject.SetActive(true);
            }

            return res;
        }
    }
}