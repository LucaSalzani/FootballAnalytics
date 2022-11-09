using System.Data;
using System.Data.SQLite;
using Dapper;
using FootballAnalytics.Application.GetAllGames;
using FootballAnalytics.Application.UpdateGamesWithLatest;
using FootballAnalytics.Domain.Entities;
using FootballAnalytics.Infrastructure.Configuration;

namespace FootballAnalytics.Infrastructure
{
    public sealed class GameRepository : IGetAllGamesQueryHandlerRepository, IUpdateGamesWithLatestCommandHandlerRepository
    {
        private readonly string _connectionString;
        public GameRepository(ConnectionStringConfiguration connectionString)
        {
            _connectionString = connectionString.LocalSqliteConnection;
        }

        public async Task<IEnumerable<Game>> GetAllGames()
        {
            EnsureDbExists();
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            const string query = @"SELECT * FROM ""Game""";
            return await connection.QueryAsync<Game>(query);
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

        private void EnsureDbExists() // Todo: Add as command? in infrastructure
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
