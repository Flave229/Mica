using Mica.Core.Communication.Models;
using RestSharp;

namespace Mica.Core.Communication
{
    public interface ISteamClient
    {
        IRestResponse Connect(string baseUrl, string resource);
        string GetSteam64IdCode(string vanityName);
        PlayerStatsForGame GetUserAchievementsForGame(string appId, string userId);
    }
}