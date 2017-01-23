using System;
using System.Data;

namespace Mica.Core.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public string ApiName { get; set; }
        public string Name { get; set; }
        public string GameName { get; set; }
        public string Description { get; set; }
        public string AchievementUrl { get; set; }
        public string IconUrl { get; set; }
        public DateTime Achieved { get; set; }

        public static Achievement Create(IDataRecord record)
        {
            return new Achievement
            {
                Id = (int)record["id"],
                ApiName = (string)record["ApiName"],
                Name = (string)record["Name"],
                GameName = (string)record["GameName"],
                Description = (string)record["Description"],
                AchievementUrl = (string)record["AchievementUrl"],
                IconUrl = (string)record["IconUrl"],
                Achieved = (DateTime)record["Achieved"]
            };
        }
    }
}