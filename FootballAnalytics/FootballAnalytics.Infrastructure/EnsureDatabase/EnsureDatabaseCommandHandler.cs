using System.Data;
using System.Data.SQLite;
using Conqueror;
using Dapper;
using FootballAnalytics.Application.Middlewares;
using FootballAnalytics.Infrastructure.Configuration;

namespace FootballAnalytics.Infrastructure.EnsureDatabase;

public sealed class EnsureDatabaseCommandHandler : IEnsureDatabaseCommandHandler, IConfigureCommandPipeline
{
    private readonly string _connectionString;

    public EnsureDatabaseCommandHandler(ConnectionStringConfiguration connectionString)
    {
        _connectionString = connectionString.LocalSqliteConnection;
    }
    
    public async Task ExecuteCommand(EnsureDatabaseCommand command, CancellationToken cancellationToken)
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
        await connection.ExecuteAsync(sql, cancellationToken);
    }
    
    public static void ConfigurePipeline(ICommandPipelineBuilder pipeline)
    {
        pipeline.UseDefault();
    }
}