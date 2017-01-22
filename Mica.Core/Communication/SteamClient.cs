using System;
using System.IO;
using System.Xml.Serialization;
using Mica.Core.Communication.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Mica.Core.Communication
{
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

        public IRestResponse Connect(string baseUrl, string resource)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(resource, Method.GET);

            return client.Execute(request);
        }

        public string GetSteam64IdCode(string vanityName)
        {
            var userCodeResponse = Connect("http://api.steampowered.com", $"ISteamUser/ResolveVanityURL/v0001/?key={_authKey}&vanityUrl={vanityName}");
            var userCodeInfo = JsonConvert.DeserializeObject<VanityUrlResponse>(userCodeResponse.Content);

            return userCodeInfo.response.success == 0 ? "" : userCodeInfo.response.steamid;
        }

        public PlayerStatsForGame GetUserAchievementsForGame(string appId, string userName)
        {
            var userStatsResponse = Connect("http://steamcommunity.com", $"id/{userName}/stats/{appId}/achievements/?xml=1");
            var xmlSerializer = new XmlSerializer(typeof(PlayerStatsForGame));
            
            using (TextReader reader = new StringReader(userStatsResponse.Content))
            {
                return xmlSerializer.Deserialize(reader) as PlayerStatsForGame;
            }
        }

        public GameListResponse GetOwnedGameListFor(string userId)
        {
            var gameListResponse = Connect("http://api.steampowered.com", $"IPlayerService/GetOwnedGames/v0001/?key={_authKey}&steamid={userId}&format=json");
            return JsonConvert.DeserializeObject<GameListResponse>(gameListResponse.Content);
        }
    }
}
