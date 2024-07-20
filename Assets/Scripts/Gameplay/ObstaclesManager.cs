using System;
using Data;
using UnityEngine;

namespace Gameplay
{
    public class ObstaclesManager : MonoBehaviour
    {
        [SerializeField] private ObstaclesDictionary _obstacles;
        private LevelsStorage _levelsStorage;
        
        public void LoadLevel(int levelIndex, LevelsStorage levelsStorage)
        {
            _levelsStorage = levelsStorage;
            InstantiateLevel(levelsStorage.Levels[levelIndex]);
        }

        private void InstantiateLevel(LevelData levelData)
        {
            Vector2 size = _obstacles[LevelFieldType.Quad].transform.localScale;
            float additionalOffset = levelData.Field.Count % 2 == 0 ? 0 : size.x / 2;
            float leftOffset = (size.x * (levelData.Field.Count / 2)) + additionalOffset;
            float topOffset = levelData.TopOffset;

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
                        obstacle.Init(fieldData.Count);
                    }
                } 
            }
        }
    }

    [Serializable]
    public class ObstaclesDictionary : SerializableDictionary<LevelFieldType, Obstacle> {}
}
