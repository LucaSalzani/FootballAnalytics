using Microsoft.Extensions.Configuration;

namespace FootballAnalytics.Infrastructure.Configuration;

public class ConnectionStringConfiguration
{
    public string LocalSqliteConnection { get; set; }

    public static ConnectionStringConfiguration FromConfiguration(IConfiguration configuration)
    {
        var localConnectionTemplate =
            configuration.GetConnectionString("LocalSqliteConnection") ?? "NoConnectionStringFound";
        
        return new ConnectionStringConfiguration
        {
            LocalSqliteConnection = localConnectionTemplate.Replace("{PathToDbFile}",
                $"{Environment.GetEnvironmentVariable("HOME")}\\FootballAnalytics.db")
        };
    }
}