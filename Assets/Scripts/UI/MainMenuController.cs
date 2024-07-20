using System;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class MainMenuController : BaseUIController
    {
        [SerializeField] private Button _playButton;
        
        private DiContainer _container;
        private ViewManager _viewManager;
            
        [Inject]
        public void Init(ViewManager viewManager, DiContainer container)
        {
            _viewManager = viewManager;
            _container = container;
        }

        private void Start()
        {
            _playButton.onClick.AddListener(StartPlay);
        }

        private void StartPlay()
        {
            _viewManager.Hide<MainMenuController>();
            _container.Resolve<GamefieldController>();
            _viewManager.Show<GameplayPanelController>();
        }
    }

    public class MainMenuControllerFactory : BaseUIControllerFactory<MainMenuController>
    {
        public MainMenuControllerFactory(DiContainer container, Transform uiParent) : base(container, uiParent)
        {
        }

        protected override string PathToGameObject => "UI/MainMenu";
    }
}
