using System.Collections.Generic;

namespace Mica.Communication.Models
{
    public class SteamGameInfo
    {
        public SteamGame game { get; set; }
    }

    public class SteamGame
    {
        public GameStats availableGameStats { get; set; }
    }

    public class GameStats
    {
        public List<GameAchievements> achievements { get; set; }
    }

    public class GameAchievements
    {
        public string name { get; set; }
        public string displayName { get; set; }
        public string icon { get; set; }
    }
}
