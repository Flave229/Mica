using System;
using System.IO;
using RestSharp;

namespace Mica.Communication
{
    public class Steam
    {
        private static string GetAuthKey()
        {
            var authFileContents = File.ReadAllLines((AppDomain.CurrentDomain.BaseDirectory + @"Auth\AuthKey.txt"));
            return string.Join("", authFileContents);
        }

        private IRestResponse Connect(string resource)
        {
            var client = new RestClient("http://api.steampowered.com");
            var request = new RestRequest(resource, Method.GET);

            return client.Execute(request);
        }

        public string GetAchievementsForGame(int appId, string userId)
        {
            appId = 211420;
            userId = "76561198040630790";
            var authKey = GetAuthKey();

            var userStats = Connect($"ISteamUserStats/GetUserStatsForGame/v0002/?appid={appId}&key={authKey}&steamid={userId}");
            var gameInfo = Connect($"ISteamUserStats/GetSchemaForGame/v2/?appid={appId}&key={authKey}");
            
            return userStats.Content;
        }
    }
}
