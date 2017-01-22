using System;
using System.Collections.Generic;
using Mica.Core.Builders;
using Mica.Core.Communication;
using Mica.Core.Models;

namespace Mica.Core.Services
{
    public class SteamService
    {
        private ISteamClient _steamClient;

        public SteamService() : this(new SteamClient()) {}

        public SteamService(ISteamClient steamClient)
        {
            _steamClient = steamClient;
        }

        public List<Achievement> UpdateSteamAchievementsFor(string appId, string userId)
        {
            var steamBuilder = new SteamAchievementBuilder(_steamClient);
            var achievements = steamBuilder.Build(appId, userId);

            return achievements;
        }
    }
}
