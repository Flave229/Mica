using System;
using System.IO;
using System.Linq;
using Mica.Communication.Models;
using Newtonsoft.Json;
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

        public string GetUserAchievementsForGame(int appId, string userId)
        {
            appId = 292030;
            //appId = 211420;
            userId = "76561198040630790";
            var authKey = GetAuthKey();
            
            var userStatsResponse = Connect($"ISteamUserStats/GetPlayerAchievements/v0001/?appid={appId}&key={authKey}&steamid={userId}");
            var userStats = JsonConvert.DeserializeObject<SteamGameStats>(userStatsResponse.Content);

            var gameInfoResponse = Connect($"ISteamUserStats/GetSchemaForGame/v2/?appid={appId}&key={authKey}");
            var gameInfo = JsonConvert.DeserializeObject<SteamGameInfo>(gameInfoResponse.Content);

            foreach (var achievement in userStats.playerstats.achievements)
            {
                if (achievement.achieved == 1)
                    continue;

                gameInfo.game.availableGameStats.achievements.RemoveAll(x => x.name == achievement.apiname);
            }

            return "";
        }
    }
}
