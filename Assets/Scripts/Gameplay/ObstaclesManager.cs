using System;
using System.Linq;
using Data;
using UnityEngine;

namespace Gameplay
{
    public class ObstaclesManager : MonoBehaviour
    {
        public event Action<int> OnObstacleDestroy;
        public event Action OnLevelCompleted;
        
        [SerializeField] private ObstaclesDictionary _obstacles;
        [SerializeField] private int _scorePerObstacle;
        
        private int _destroyedObstaclesCount;
        private int _totalObstaclesCount;

        public float LevelProgress => _destroyedObstaclesCount / (float)_totalObstaclesCount;
        
        public void LoadLevel(int levelIndex, LevelsStorage levelsStorage)
        {
            InstantiateLevel(levelsStorage.Levels[levelIndex]);
        }

        private void InstantiateLevel(LevelData levelData)
        {
            Vector2 size = _obstacles[LevelFieldType.Quad].transform.localScale;
            float additionalOffset = levelData.Field.Count % 2 == 0 ? 0 : size.x / 2;
            float leftOffset = (size.x * (levelData.Field.Count / 2)) + additionalOffset;
            float topOffset = levelData.TopOffset;
            int maxVal = levelData.Field.Max(f => f.Column.Max(c => c.Count));

            for (int i = 0; i < levelData.Field.Count; i++)
            {
                for (int j = 0; j < levelData.Field[i].Column.Count; j++)
                {
                    Field fieldData = levelData.Field[i].Column[j];
                    if (fieldData.Exist)
                    {
                        _totalObstaclesCount++;
                        Obstacle obstacle = Instantiate(_obstacles[fieldData.Type], transform);
                        float xPos = -leftOffset + i * size.x + size.x / 2;
                        float yPos = topOffset - j * size.y - size.y / 2;
                        obstacle.transform.localPosition = new Vector3(xPos, yPos, 0);
                        obstacle.Init(fieldData.Count,
                            levelData.Gradient.Evaluate(fieldData.Count / (float)maxVal), ObstacleDestroy);
                    }
                } 
            }
        }

        private void ObstacleDestroy()
        {
            _destroyedObstaclesCount++;
           OnObstacleDestroy?.Invoke(_scorePerObstacle);
           if (_destroyedObstaclesCount == _totalObstaclesCount)
           {
               OnLevelCompleted?.Invoke();
           }
        }
    }

    [Serializable]
    public class ObstaclesDictionary : SerializableDictionary<LevelFieldType, Obstacle> {}
}
