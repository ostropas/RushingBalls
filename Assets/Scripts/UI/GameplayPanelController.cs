using System;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GameplayPanelController : BaseUIController
    {
        [SerializeField] private TMP_Text _score;
        [SerializeField] private Slider _scoreSlider;
        [SerializeField] private Button _pauseButton;
        
        private GamefieldController _gamefieldController;
        private ViewManager _viewManager;

        [Inject]
        public void Init(GamefieldController gamefieldController, ViewManager viewManager)
        {
            _gamefieldController = gamefieldController;
            _viewManager = viewManager;
           gamefieldController.ScoreUpdated += GamefieldControllerOnAddedScore;
        }

        private void Start()
        {
           _score.text = "0";
           _scoreSlider.value = 0;
           _pauseButton.onClick.AddListener(ReturnToMainMenu);
        }

        private void ReturnToMainMenu()
        {
            Destroy(_gamefieldController.gameObject);
            _viewManager.Show<MainMenuController>();
            _viewManager.Hide<GameplayPanelController>();
        }


        private void GamefieldControllerOnAddedScore()
        {
            _scoreSlider.value = _gamefieldController.LevelProgress;
            _score.text = _gamefieldController.Score.ToString();
        }
    }
}