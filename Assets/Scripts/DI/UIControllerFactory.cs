using UI;
using UnityEngine;
using Zenject;

namespace DI
{
    public class UIControllerFactory
    {
        public class BaseUIControllerFactory<T> : IFactory<T> where T : BaseUIController
        {
            private readonly DiContainer _container;
            private readonly Transform _uiParent;
            private readonly string _path;
            
            public BaseUIControllerFactory(DiContainer container, Transform uiParent, string path)
            {
                _container = container;
                _uiParent = uiParent;
                _path = path;
            }
            
            public T Create()
            {
                T prefab = Resources.Load<T>(_path);
                T obj = _container.InstantiatePrefabForComponent<T>(prefab, _uiParent);
                return obj;
            }
        }
    }
}