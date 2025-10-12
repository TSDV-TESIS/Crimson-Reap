using System;
using UnityEngine;
using UnityEngine.Events;

namespace Leaderboard
{
    public enum LevelEnum
    {
        Level1 = 0,
        Level2,
        Level3,
        Level4
    }
    
    [CreateAssetMenu(menuName = "Leaderboard data", fileName = "LeaderboardData")]
    public class LeaderboardData : ScriptableObject
    {
        public bool isLoading;
        public bool hasError;
        public string leaderboardLevel;
        public LeaderboardRow[] data;
        public UnityEvent<LevelEnum> requestData;
    }
}
