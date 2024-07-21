using System;
using System.Collections.Generic;
using TMPro;
using UI.InfiniteScroll;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Leaderboard
{
    public class LeaderboardElement : InfiniteScrollElement<LeaderboardData>
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _scoreVal;
        [SerializeField] private TMP_Text _posVal;
        [SerializeField] private Image _medalImage;
        [SerializeField] private List<Sprite> _medalsSprites;
        [SerializeField] private Image _backImage;
        [SerializeField] private Color _playerBackColor;

        private Color _defaultBackColor;

        private void Awake()
        {
            _defaultBackColor = _backImage.color;
        }

        public override void ResetView()
        {
            _name.text = "";
            _scoreVal.text = "";
            _posVal.text = "";
            _medalImage.gameObject.SetActive(true);
            _backImage.color = _defaultBackColor;
        }

        public override void Init(LeaderboardData data)
        {
            _name.text = data.Name;
            _scoreVal.text = data.Score.ToString();
            _posVal.text = (data.Pos + 1).ToString();
            if (data.Pos < _medalsSprites.Count)
            {
                _medalImage.sprite = _medalsSprites[data.Pos];
            }
            else
            {
                _medalImage.gameObject.SetActive(false);
            }

            if (data.IsPlayer)
            {
                _backImage.color = _playerBackColor;
            }
        }
    }
}