using System.Threading.Tasks;
using Data;
using Gameplay;

namespace Controllers
{
    public class PlayerDataController
    {
        private readonly PlayerDataSaver _playerDataSaver;
        private const string SavePath = "data.json";
        public PlayerData PlayerData => _playerDataSaver.PlayerData;

        public int LastLevelScore { get; private set; }
        public int LastTookMultiplier { get; private set; }

        public PlayerDataController()
        {
            _playerDataSaver = new PlayerDataSaver(SavePath);
            _playerDataSaver.Load();
        }

        public void LevelCompleted(int score)
        {
            LastLevelScore = score;
        }

        public async Task ApplyScoreAndMultiplier(int multiplier)
        {
            LastTookMultiplier = multiplier;
            LastLevelScore *= multiplier;
            PlayerData.Score += LastLevelScore;
            LastLevelScore = 0;
            PlayerData.CurrentLevel++;
            await _playerDataSaver.Save();
        }
    }
}