using System.Data;
using System.Data.SQLite;
using Dapper;
using FootballAnalytics.Application.GetAllGames;
using FootballAnalytics.Application.UpdateGamesWithLatest;
using FootballAnalytics.Domain.Entities;
using FootballAnalytics.Infrastructure.Configuration;
using FootballAnalytics.Infrastructure.EnsureDatabase;

namespace FootballAnalytics.Infrastructure
{
    public sealed class GameRepository : IGetAllGamesQueryHandlerRepository, IUpdateGamesWithLatestCommandHandlerRepository
    {
        private readonly IEnsureDatabaseCommandHandler _ensureDatabaseCommandHandler;
        private readonly string _connectionString;
        public GameRepository(ConnectionStringConfiguration connectionString, IEnsureDatabaseCommandHandler ensureDatabaseCommandHandler)
        {
            _ensureDatabaseCommandHandler = ensureDatabaseCommandHandler;
            _connectionString = connectionString.LocalSqliteConnection;
        }

        public async Task<IEnumerable<Game>> GetAllGames(CancellationToken cancellationToken)
        {
            await EnsureDbExists(cancellationToken);
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            const string query = @"SELECT * FROM ""Game""";
            return await connection.QueryAsync<Game>(query);
        }

        public async Task UpsertGamesByGameNumber(IEnumerable<Game> games, CancellationToken cancellationToken)
        {
            await EnsureDbExists(cancellationToken);
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
                await connection.ExecuteAsync(upsert, game);
            }
        }

        private async Task EnsureDbExists(CancellationToken cancellationToken)
        {
            await _ensureDatabaseCommandHandler.ExecuteCommand(new(), cancellationToken);
        }
    }
}
