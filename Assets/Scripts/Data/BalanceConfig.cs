using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Balance", menuName = "Create balance config", order = 0)]
    public class BalanceConfig : ScriptableObject
    {
        public List<int> Multipliers;
        public int BallsCount;
        public int MaxGameplayScoreMultiplier;
    }
}