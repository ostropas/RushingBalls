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
        
        private GamefieldController _gamefieldController;

        [Inject]
        public void Init(GamefieldController gamefieldController)
        {
            _gamefieldController = gamefieldController;
           gamefieldController.ScoreUpdated += GamefieldControllerOnAddedScore;
           _score.text = "0";
           _scoreSlider.value = 0;
        }

        private void GamefieldControllerOnAddedScore(int obj)
        {
            _scoreSlider.value = (obj / (float)_gamefieldController.MaxScore);
            _score.text = obj.ToString();
        }
    }
    

    public class GameplayPanelControllerFactory : BaseUIControllerFactory<GameplayPanelController >
    {
        public GameplayPanelControllerFactory(DiContainer container, Transform uiParent) : base(container, uiParent)
        {
        }

        protected override string PathToGameObject => "UI/TopGameplayPanel";
    }
}