using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create level storage", fileName = "LevelStorage")]
    public class LevelsStorage : ScriptableObject
    {
        public List<LevelData> Levels;
    }
}
