using System;
using System.IO;
using Mica.Core.Communication.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Mica.Core.Communication
{
    public interface ISteamClient
    {
        IRestResponse Connect(string resource);
        SteamGameStats GetUserAchievementsForGame(string appId, string userId);
        SteamGameInfo GetInfoForGame(string appId);
    }

    public class SteamClient : ISteamClient
    {
        private readonly string _authKey;

        public SteamClient()
        {
            _authKey = GetAuthKey();
        }

        private static string GetAuthKey()
        {
            var authFileContents = File.ReadAllLines((AppDomain.CurrentDomain.BaseDirectory + @"Auth\AuthKey.txt"));
            return string.Join("", authFileContents);
        }

        public IRestResponse Connect(string resource)
        {
            var client = new RestClient("http://api.steampowered.com");
            var request = new RestRequest(resource, Method.GET);

            return client.Execute(request);
        }

        public SteamGameStats GetUserAchievementsForGame(string appId, string userId)
        {
            var userStatsResponse = Connect($"ISteamUserStats/GetPlayerAchievements/v0001/?appid={appId}&key={_authKey}&steamid={userId}");
            var userStats = JsonConvert.DeserializeObject<SteamGameStats>(userStatsResponse.Content);
            
            return userStats;
        }

        public SteamGameInfo GetInfoForGame(string appId)
        {
            var gameInfoResponse = Connect($"ISteamUserStats/GetSchemaForGame/v2/?appid={appId}&key={_authKey}");
            var gameInfo = JsonConvert.DeserializeObject<SteamGameInfo>(gameInfoResponse.Content);

            return gameInfo;
        }
    }
}
