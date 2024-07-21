using System.Collections.Generic;
using System.Linq;
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

        public T Show<T>() where T : BaseUIController
        {
            BaseUIController controller = _currentUIControllers.FirstOrDefault(x => x is T);
            if (controller != null)
            {
                return controller as T;
            }
            
            T controllerInstance = _container.Resolve<T>();
            _currentUIControllers.Add(controllerInstance);
            controllerInstance.Show();
            return controllerInstance;
        }

        public void Hide<T>()
        {
            BaseUIController controller = _currentUIControllers.FirstOrDefault(x => x is T);
            if (controller != null)
            {
                _currentUIControllers.Remove(controller);
                controller.Hide();
            }
        }
    }
}
