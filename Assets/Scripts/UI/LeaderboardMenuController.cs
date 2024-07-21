using System.Collections.Generic;
using System.Linq;
using UI.Leaderboard;
using UnityEngine;

namespace UI
{
    public class LeaderboardMenuController : BaseUIController
    {
        [SerializeField] private LeaderboardScroll _scroll;

        private void Start()
        {
            List<LeaderboardData> data = new()
            {
                new LeaderboardData()
                {
                    IsPlayer = true,
                    Name = "Player",
                    Score = 500
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

            for (int i = 0; i < 99; i++)
            {
                data.Add(new LeaderboardData()
                {
                    IsPlayer = false,
                    Name = names[Random.Range(0, names.Count)],
                    Score = Random.Range(400, 600)
                });
            }
            
            data = data.OrderBy(x => x.Score).ToList();
            for (int index = 0; index < data.Count; index++)
            {
                LeaderboardData leaderboardData = data[index];
                leaderboardData.Pos = index;
            }

            _scroll.Init(data);
        }
    }
}