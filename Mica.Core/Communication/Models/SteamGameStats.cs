using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace Mica.Core.Communication.Models
{
    [Serializable]
    [XmlRoot("playerstats")]
    public class SteamGameStats
    {
        [XmlArray("achievements")]
        [XmlArrayItem("achievement", typeof(PlayerAchievement))]
        public List<PlayerAchievement> Achievements { get; set; }

        [XmlElement("game", typeof(GameInfo))]
        public GameInfo GameInfo { get; set; }
    }

    public class GameInfo
    {
        [XmlElement("gameFriendlyName")]
        public string ApplicationId { get; set; }

        [XmlElement("gameName")]
        public string Name { get; set; }

        [XmlElement("gameIcon")]
        public string IconUrl { get; set; }
    }

    public class PlayerAchievement
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("apiname")]
        public string ApiName { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("iconClosed")]
        public string IconUrl { get; set; }

        [XmlElement("unlockTimestamp")]
        public string AchievedTimestamp { get; set; }

        [XmlAttribute("closed")]
        public int Achieved { get; set; }
    }
}
