using System;
using System.Collections.Generic;
using System.Linq;
using Mica.Core.Communication;
using Mica.Core.Models;

namespace Mica.Core.Builders
{
    public class SteamAchievementBuilder
    {
        private readonly ISteamClient _steamClient;

        public SteamAchievementBuilder() : this(new SteamClient()) {}

        public SteamAchievementBuilder(ISteamClient steamClient)
        {
            _steamClient = steamClient;
        }

        public List<Achievement> Build(string appId, string userName)
        {
            var achievements = _steamClient.GetUserAchievementsForGame(appId, userName);
            var earnedAchievements = new List<Achievement>();

            foreach (var achievement in achievements.Achievements)
            {
                if (achievement.Achieved == 0)
                    continue;

                earnedAchievements.Add(new Achievement
                {
                    AchievementName = achievement.Name,
                    GameName = achievements.GameInfo.Name,
                    AchievementUrl = $"http://steamcommunity.com/id/flave_229/stats/{appId}/achievements/",
                    IconUrl = achievement.IconUrl,
                    Achieved = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(double.Parse(achievement.AchievedTimestamp))
                });
            }

            return earnedAchievements;
        }
    }
}
