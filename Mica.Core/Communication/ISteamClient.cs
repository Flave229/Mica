using Mica.Core.Communication.Models;
using RestSharp;

namespace Mica.Core.Communication
{
    public interface ISteamClient
    {
        IRestResponse Connect(string baseUrl, string resource);
        SteamGameStats GetUserAchievementsForGame(string appId, string userId);
    }
}