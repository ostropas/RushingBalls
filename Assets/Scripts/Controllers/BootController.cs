using UI;
using Zenject;

namespace Controllers
{
    public class BootController
    {
        private readonly ViewManager _viewManager;
        private readonly DiContainer _container;

        public BootController(ViewManager viewManager, DiContainer container)
        {
            _viewManager = viewManager;
            _container = container;
        }

        public void StartGame()
        {
            _viewManager.Show<MainMenuController>();
        }

        public void StartLevel()
        {
            _container.Resolve<LevelController>();
        } 
    }
}