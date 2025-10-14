using System;

namespace Leaderboard
{
    [Serializable]
    public class GetLeaderboardResponse
    {
        public LeaderboardRow[] data;
    }
}