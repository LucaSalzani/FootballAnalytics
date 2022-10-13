using System.Data;
using System.Data.SQLite;
using Dapper;
using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace FootballAnalytics.Infrastructure
{
    public class GameRepository : IGameRepository
    {
        private readonly string _connectionString;
        public GameRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("LocalSqliteConnection");
        }
        
        public void StoreGames(IEnumerable<Game> games)
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            foreach (var game in games)
            {
                var statement = @"INSERT INTO ""Game"" (""GameNumber"",""GameDateBinary"",""HomeTeam"",""AwayTeam"",""HomeTeamGoals"",""AwayTeamGoals"",""LinkToGame"") VALUES (@GameNumber, @GameDateBinary, @HomeTeam, @AwayTeam, @HomeTeamGoals, @AwayTeamGoals, @LinkToGame);";
                connection.Execute(statement, game);
            }
        }
    }
}
