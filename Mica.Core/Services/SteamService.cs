using System;
using System.Collections.Generic;
using Mica.Core.Builders;
using Mica.Core.Communication;
using Mica.Core.Models;
using Mica.Core.Repositories;

namespace Mica.Core.Services
{
    public class SteamService
    {
        private readonly ISteamClient _steamClient;
        private readonly ISteamRepository _steamRepository;

        public SteamService() : this(new SteamClient(), new SteamRepository()) {}

        public SteamService(ISteamClient steamClient, ISteamRepository steamRepository)
        {
            _steamClient = steamClient;
            _steamRepository = steamRepository;
        }

        public List<Achievement> UpdateSteamAchievementsFor(string appId, string userId)
        {
            var steamBuilder = new SteamAchievementBuilder(_steamClient, _steamRepository);
            var achievements = steamBuilder.BuildFor(appId, userId);

            return achievements;
        }
    }
}
