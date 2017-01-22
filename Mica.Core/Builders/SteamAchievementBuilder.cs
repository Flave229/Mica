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

        public List<Achievement> Build(string appId, string userId)
        {
            var achievements = _steamClient.GetUserAchievementsForGame(appId, userId);
            var gameInfo = _steamClient.GetInfoForGame(appId);
            var earnedAchievements = new List<Achievement>();

            foreach (var achievement in achievements.playerstats.achievements)
            {
                if (achievement.achieved == 0)
                    continue;

                var achievementInfo = gameInfo.game.availableGameStats.achievements.FirstOrDefault(x => x.name == achievement.apiname);

                earnedAchievements.Add(new Achievement
                {
                    AchievementName = achievementInfo?.displayName,
                    GameName = achievements.playerstats.gameName,
                    AchievementUrl = $"http://steamcommunity.com/id/flave_229/stats/{appId}/achievements/",
                    IconUrl = achievementInfo?.icon
                });
            }

            return earnedAchievements;
        }
    }
}
