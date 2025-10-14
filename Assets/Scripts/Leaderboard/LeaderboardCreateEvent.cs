using UnityEngine;
using UnityEngine.Events;

namespace Leaderboard
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Create leaderboard data", fileName = "Create Leaderboard Data")]
    public class LeaderboardCreateEvent : ScriptableObject
    {
        public bool hasError;
        public UnityEvent<LeaderboardRow> createNewTime;
        public UnityEvent createNewTimeFinish;
    }
}