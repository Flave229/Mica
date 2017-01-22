using System;
using System.Data;

namespace Mica.Core.Repositories.SqlModels
{
    public class MicaGame
    {
        public int Id { get; set; }
        public string AppId { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public DateTime Added { get; set; }
        public DateTime Updated { get; set; }

        public static MicaGame Create(IDataRecord record)
        {
            return new MicaGame
            {
                Id = (int)record["id"],
                AppId = (string)record["AppId"],
                Name = (string)record["Name"],
                IconUrl = (string)record["IconUrl"],
                Added = (DateTime)record["Added"],
                Updated = (DateTime)record["Updated"]
            };
        }
    }
}
