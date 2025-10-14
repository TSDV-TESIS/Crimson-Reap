using System;

namespace Leaderboard
{
    [Serializable]
    public class LeaderboardRow
    {
        public static string GetLevelNameByLevelEnum(LevelEnum level)
        {
            switch (level)
            {
                case LevelEnum.Level1:
                    return LevelNames.Level1;
                case LevelEnum.Level2:
                    return LevelNames.Level2;
                case LevelEnum.Level3:
                    return LevelNames.Level3;
                case LevelEnum.Level4:
                    return LevelNames.Level4;
                default:
                    return "";
            }
        }
        public LeaderboardRow(string aName, int aTime, LevelEnum aLevel)
        {
            name = aName;
            timeBeaten = aTime;
            level = GetLevelNameByLevelEnum(aLevel);
        }
        
        public string name;
        public int timeBeaten;
        public string level;
    }
}