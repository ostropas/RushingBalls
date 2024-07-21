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
        
        private ViewManager _viewManager;
            
        [Inject]
        public void Init(ViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        private void Start()
        {
            _playButton.onClick.AddListener(StartPlay);
        }

        private void StartPlay()
        {
            _viewManager.Hide<MainMenuController>();
            _viewManager.Show<GameplayPanelController>();
        }
    }
}
