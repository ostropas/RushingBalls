using UI;

namespace Controllers
{
    public class BootController
    {
        private readonly ViewManager _viewManager;

        public BootController(ViewManager viewManager)
        {
            _viewManager = viewManager;
           StartGame(); 
        }

        private void StartGame()
        {
            _viewManager.Show<MainMenuController>();
        }
    }
}