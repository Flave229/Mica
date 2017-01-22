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

        public List<Achievement> BuildAll(string userName)
        {
            var userId = _steamClient.GetSteam64IdCode(userName);
            var games = _steamClient.GetOwnedGameListFor(userId);

            var achievements = new List<Achievement>();

            foreach (var game in games.response.games)
            {
                achievements.AddRange(BuildFor(game.appid.ToString(), userName));
            }

            return achievements.OrderByDescending(x => x.Achieved).ToList();
        }

        public List<Achievement> BuildFor(string appId, string userName)
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

            return earnedAchievements.OrderByDescending(x => x.Achieved).ToList();
        }
    }
}
