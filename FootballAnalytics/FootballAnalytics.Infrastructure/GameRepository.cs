using System.Data;
using System.Data.SQLite;
using Dapper;
using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Domain.Entities;
using FootballAnalytics.Infrastructure.Configuration;

namespace FootballAnalytics.Infrastructure
{
    public class GameRepository : IGameRepository
    {
        private readonly string _connectionString;
        public GameRepository(ConnectionStringConfiguration connectionString)
        {
            _connectionString = connectionString.LocalSqliteConnection;
        }

        public IEnumerable<Game> GetAllGames()
        {
            EnsureDbExists();
            using IDbConnection connection = new SQLiteConnection(_connectionString); // TODO: always create connection necessary?
            const string query = @"SELECT * FROM ""Game""";
            return connection.Query<Game>(query);
        }

        public void UpsertGamesByGameNumber(IEnumerable<Game> games)
        {
            EnsureDbExists();
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            foreach (var game in games)
            {
                const string upsert = @"INSERT INTO ""Game"" (""GameNumber"",""GameDateBinary"",""HomeTeam"",""AwayTeam"",""HomeTeamGoals"",""AwayTeamGoals"",""LinkToGame"")
                        VALUES (@GameNumber, @GameDateBinary, @HomeTeam, @AwayTeam, @HomeTeamGoals, @AwayTeamGoals, @LinkToGame)
                    ON CONFLICT(""GameNumber"") DO UPDATE SET
                        GameNumber = excluded.GameNumber,
                        GameDateBinary = excluded.GameDateBinary,
                        HomeTeam = excluded.HomeTeam,
                        AwayTeam = excluded.AwayTeam,
                        HomeTeamGoals = excluded.HomeTeamGoals,
                        AwayTeamGoals = excluded.AwayTeamGoals,
                        LinkToGame = excluded.LinkToGame;";
                connection.Execute(upsert, game);
            }
        }

        private void EnsureDbExists()
        {
            var dbFile = $"{Environment.GetEnvironmentVariable("HOME")}\\FootballAnalytics.db";
            if (File.Exists(dbFile))
            {
                return;
            }

            SQLiteConnection.CreateFile(dbFile);

            using IDbConnection connection = new SQLiteConnection(_connectionString);
            const string sql = @"CREATE TABLE IF NOT EXISTS ""Game"" (
            ""Id"" INTEGER NOT NULL UNIQUE,
            ""GameNumber"" INTEGER NOT NULL UNIQUE,
            ""GameDateBinary"" INTEGER NOT NULL,
            ""HomeTeam"" TEXT NOT NULL,
            ""AwayTeam"" TEXT NOT NULL,
            ""HomeTeamGoals"" INTEGER,
            ""AwayTeamGoals"" INTEGER,
            ""LinkToGame"" TEXT,
            PRIMARY KEY(""Id"" AUTOINCREMENT));";
            connection.Execute(sql);
        }
    }
}
