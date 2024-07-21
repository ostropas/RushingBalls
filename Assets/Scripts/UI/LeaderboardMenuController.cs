using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using UI.Leaderboard;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace UI
{
    public class LeaderboardMenuController : BaseUIController
    {
        [SerializeField] private LeaderboardScroll _scroll;
        [SerializeField] private Button _playAgainButton;
        
        private PlayerDataController _playerDataController;
        private ViewManager _viewManager;

        private void Awake()
        {
            _playAgainButton.onClick.AddListener(PlayAgain);
        }

        [Inject]
        public void Init(PlayerDataController playerDataController, ViewManager viewManager)
        {
            _playerDataController = playerDataController;
            _viewManager = viewManager;
        }

        private void Start()
        {
            List<LeaderboardData> data = new()
            {
                new LeaderboardData()
                {
                    IsPlayer = true,
                    Name = "Player",
                    Score = _playerDataController.PlayerData.Score
                }
            };

            List<string> names = new()
            {
                "Alex",
                "John",
                "Sam",
                "Mark",
                "Bob"
            };

            int minScore = Mathf.Max(_playerDataController.PlayerData.Score - 400, 0);
            int maxScore = _playerDataController.PlayerData.Score;
            if (_playerDataController.LastTookMultiplier == 1)
            {
                maxScore += 100;
            }

            for (int i = 0; i < 99; i++)
            {
                data.Add(new LeaderboardData()
                {
                    IsPlayer = false,
                    Name = names[Random.Range(0, names.Count)],
                    Score = Random.Range(minScore, maxScore)
                });
            }
            
            data = data.OrderByDescending(x => x.Score).ToList();
            for (int index = 0; index < data.Count; index++)
            {
                LeaderboardData leaderboardData = data[index];
                leaderboardData.Pos = index;
            }

            _scroll.Init(data);
        }
        
        private void PlayAgain()
        {
            _viewManager.Hide<LeaderboardMenuController>();
            _viewManager.Show<GameplayPanelController>();
        }
    }
}