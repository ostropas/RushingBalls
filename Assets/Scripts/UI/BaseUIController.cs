using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI
{
    public abstract class BaseUIController : MonoBehaviour
    {
        public virtual Task Show()
        {
            return Task.CompletedTask;
        }

        public virtual Task Hide()
        {
           Destroy(gameObject);
           return Task.CompletedTask;
        }
    }
    
    public abstract class BaseUIControllerFactory<T> : IFactory<T> where T : BaseUIController
    {
        private readonly DiContainer _container;
        private readonly Transform _uiParent;
        
        public BaseUIControllerFactory(DiContainer container, Transform uiParent)
        {
            _container = container;
            _uiParent = uiParent;
        }
        
        public T Create()
        {
            T prefab = Resources.Load<T>(PathToGameObject);
            T obj = _container.InstantiatePrefabForComponent<T>(prefab, _uiParent);
            return obj;
        }
        
        protected abstract string PathToGameObject { get; }
    }
}
