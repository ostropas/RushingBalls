using System;
using Controllers;
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
        private BootController _bootController;
            
        [Inject]
        public void Init(ViewManager viewManager, BootController bootController)
        {
            _viewManager = viewManager;
            _bootController = bootController;
        }

        private void Start()
        {
            _playButton.onClick.AddListener(StartPlay);
        }

        private void StartPlay()
        {
            _viewManager.Hide<MainMenuController>();
            _bootController.StartLevel();
        }
    }
}
