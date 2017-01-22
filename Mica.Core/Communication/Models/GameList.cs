using System.Collections.Generic;

namespace Mica.Core.Communication.Models
{
    public class GameList
    {
        public int game_count { get; set; }
        public List<SteamGame> games { get; set; }
    }
}