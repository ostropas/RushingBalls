using System.Collections.Generic;
using System.Threading.Tasks;
using Controllers;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class MultiplierMenuController : BaseUIController
    {
        [SerializeField] private TMP_Text _descText;
        [SerializeField] private List<ButtonWithText> _multiButtons;
        
        private PlayerDataController _playerDataController;
        private ViewManager _viewManager;
        
        private bool _multiplierClicked;
        
        [Inject]
        public void Init(PlayerDataController playerDataController, ViewManager viewManager)
        {
            _playerDataController = playerDataController;
            _viewManager = viewManager;
        }

        private void Start()
        {
            _descText.text = string.Format(_descText.text, _playerDataController.LastLevelScore);
            List<int> muls = new List<int>() { 1, 3, 5 };
            for (var index = 0; index < _multiButtons.Count; index++)
            {
                ButtonWithText multiButton = _multiButtons[index];
                int mulVal = muls[index];

                multiButton.Text.text = $"X{mulVal.ToString()}";
                int indexCopy = index;
                multiButton.Button.onClick.AddListener(() => _ = ApplyMul(muls[indexCopy]));
            }
        }

        private async Task ApplyMul(int mul)
        {
            if (_multiplierClicked) return;
            _multiplierClicked = true;
            await _playerDataController.ApplyScoreAndMultiplier(mul);
            _viewManager.Hide<MultiplierMenuController>();
            _viewManager.Show<LeaderboardMenuController>();
        }
    }
    
    public class MultiplierMenuControllerFactory : BaseUIControllerFactory<MultiplierMenuController>
    {
        public MultiplierMenuControllerFactory(DiContainer container, Transform uiParent) : base(container, uiParent)
        {
        }

        protected override string PathToGameObject => "UI/MultiplierMenu";
    }
}
