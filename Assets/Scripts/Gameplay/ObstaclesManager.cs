using System;
using System.Linq;
using Data;
using UnityEngine;

namespace Gameplay
{
    public class ObstaclesManager : MonoBehaviour
    {
        public event Action<int> OnObstacleDestroy;
        
        [SerializeField] private ObstaclesDictionary _obstacles;
        [SerializeField] private int _scorePerObstacle;
        
        private int _destroyedObstaclesCount;

        public int MaxScore { get; private set; } = 0;
        
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

            MaxScore = levelData.Field.Count * levelData.Field[0].Column.Count * _scorePerObstacle;
            for (int i = 0; i < levelData.Field.Count; i++)
            {
                for (int j = 0; j < levelData.Field[i].Column.Count; j++)
                {
                    Field fieldData = levelData.Field[i].Column[j];
                    if (fieldData.Exist)
                    {
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
           OnObstacleDestroy?.Invoke(++_destroyedObstaclesCount * _scorePerObstacle); 
        }
    }

    [Serializable]
    public class ObstaclesDictionary : SerializableDictionary<LevelFieldType, Obstacle> {}
}
