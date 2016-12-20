using System.Collections.Generic;
using System.Linq;
using Mica.Communication;
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

                earnedAchievements.Add(new Achievement
                {
                    AchievementName = gameInfo.game.availableGameStats.achievements.FirstOrDefault(x => x.name == achievement.apiname)?.displayName,
                    GameName = achievements.playerstats.gameName
                });
                gameInfo.game.availableGameStats.achievements.RemoveAll(x => x.name == achievement.apiname);
            }

            return earnedAchievements;
        }
    }
}
