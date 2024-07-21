using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameplayPanelController : BaseUIController
    {
        [SerializeField] private TMP_Text _score;
        [SerializeField] private Slider _scoreSlider;
        [SerializeField] private Button _pauseButton;

        public void SetOnExit(Action onExit)
        {
           _pauseButton.onClick.AddListener(() => onExit?.Invoke());
        }

        public void OnUpdateData(float levelProgress, int score)
        {
            _scoreSlider.value = levelProgress;
            _score.text = score.ToString();
        }

        private void Start()
        {
           _score.text = "0";
           _scoreSlider.value = 0;
        }
    }
}