using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using Mica.Core.Communication.Models;
using Mica.Core.Models;
using Mica.Core.Repositories.SqlModels;

namespace Mica.Core.Repositories
{
    public interface ISteamRepository
    {
        void InsertGame(GameInfo game);
        void InsertAchievements(List<Achievement> achievements);
        void GetAchievements();
        IEnumerable<MicaGame> GetGame(string gameInfoApplicationId);
    }

    public class SteamRepository : ISteamRepository
    {
        private readonly SqlConnection _connection;

        public SteamRepository()
        {
            var userFileContents = File.ReadAllLines((AppDomain.CurrentDomain.BaseDirectory + @"Auth\DatabaseUser.txt"));
            var user = string.Join("", userFileContents);
            var passwordFileContents = File.ReadAllLines((AppDomain.CurrentDomain.BaseDirectory + @"Auth\DatabasePass.txt"));
            var password = string.Join("", passwordFileContents);

            _connection = new SqlConnection($"Server=tcp:micaflave.database.windows.net,1433;Initial Catalog=MicaSQL;Persist Security Info=False;User ID={user};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public void InsertGame(GameInfo game)
        {
            try
            {
                game.Name = game.Name.Replace("'", "''");

                _connection.Open();
                var command = new SqlCommand("INSERT INTO SteamGames (AppId, Name, IconUrl) " +
                                             $"VALUES ('{game.ApplicationId}', '{game.Name}', '{game.IconUrl}')", _connection);

                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                throw new Exception("Failed to insert Steam game into the database", exception);
            }
            finally
            {
                _connection?.Close();
            }
        }

        public IEnumerable<MicaGame> GetGame(string applicationId)
        {
            SqlDataReader sqlReader = null;
            try
            {
                _connection.Open();
                var command = new SqlCommand("SELECT * FROM SteamGames " +
                                             $"WHERE AppId = '{applicationId}'", _connection);

                sqlReader = command.ExecuteReader();

                while (sqlReader.Read())
                {
                    var test = (string) sqlReader["AppId"];
                    yield return MicaGame.Create(sqlReader);
                }
            }
            finally
            {
                sqlReader?.Close();
                _connection?.Close();
            }
        }

        public void InsertAchievements(List<Achievement> achievements)
        {
            
        }

        public void GetAchievements()
        {
            try
            {
                _connection.Open();
                var command = new SqlCommand("SELECT * FROM SteamGames");
            }
            catch (Exception exception)
            {
                
            }
        }
    }
}
