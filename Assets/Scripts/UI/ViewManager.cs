using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI
{
    public class ViewManager
    {
        private HashSet<BaseUIController> _currentUIControllers = new();
        private DiContainer _container;

        public ViewManager(DiContainer container)
        {
            _container = container;
        }

        public async Task<T> Show<T>() where T : BaseUIController
        {
            BaseUIController controller = _currentUIControllers.FirstOrDefault(x => x is T);
            if (controller != null)
            {
                return controller as T;
            }
            
            T controllerInstance = _container.Resolve<T>();
            _currentUIControllers.Add(controllerInstance);
            await controllerInstance.Show();
            return controllerInstance;
        }

        public async Task Hide<T>()
        {
            BaseUIController controller = _currentUIControllers.FirstOrDefault(x => x is T);
            if (controller != null)
            {
                _currentUIControllers.Remove(controller);
                await controller.Hide();
            }
        }
    }
}
