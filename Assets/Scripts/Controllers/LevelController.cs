using Gameplay;
using UI;
using UnityEngine;

namespace Controllers
{
    // Controls data between gameplay and UI
    public class LevelController
    {
        private GameplayPanelController _topPanel;
        private GamefieldController _gamefieldController;
        private ViewManager _viewManager;
        private PlayerDataController _playerDataController;

        public LevelController(GamefieldController gamefieldController, ViewManager viewManager, PlayerDataController playerDataController)
        {
            _gamefieldController = gamefieldController;
            _topPanel = viewManager.Show<GameplayPanelController>();
            _viewManager = viewManager;
            _playerDataController = playerDataController;
            _gamefieldController.ScoreUpdated += ScoreUpdated;
            _gamefieldController.LevelCompleted += CompleteLevel;
            _topPanel.SetOnExit(ReturnToMainMenu);
            _gamefieldController.StartLevel(_playerDataController.PlayerData.CurrentLevel);
        }

        private void ScoreUpdated()
        {
            _topPanel.OnUpdateData(_gamefieldController.LevelProgress, _gamefieldController.Score);
        }

        private void CompleteLevel()
        {
           _playerDataController.LevelCompleted(_gamefieldController.Score);
           _viewManager.Hide<GameplayPanelController>();
           _viewManager.Show<MultiplierMenuController>();
            Object.Destroy(_gamefieldController.gameObject);
        }

        private void ReturnToMainMenu()
        {
            Object.Destroy(_gamefieldController.gameObject);
            _viewManager.Show<MainMenuController>();
            _viewManager.Hide<GameplayPanelController>();
        }
    }
}