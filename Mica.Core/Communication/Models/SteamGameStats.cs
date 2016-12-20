using System.Collections.Generic;

namespace Mica.Communication.Models
{
    public class SteamGameStats
    {
        public PlayerStats playerstats { get; set; }
    }

    public class PlayerStats
    {
        public string gameName { get; set; }
        public List<PlayerAchievement> achievements { get; set; }
    }

    public class PlayerAchievement
    {
        public string apiname { get; set; }
        public int achieved { get; set; }
    }
}
