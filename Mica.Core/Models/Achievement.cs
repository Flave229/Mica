using System;

namespace Mica.Core.Models
{
    public class Achievement
    {
        public string GameName { get; set; }
        public string AchievementName { get; set; }
        public string AchievementUrl { get; set; }
        public string IconUrl { get; set; }
        public DateTime Achieved { get; set; }
    }
}