using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Infrastructure;
using FootballAnalytics.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace FootballAnalytics.WebCrawler
{
    public static class DependencyInjectionSetup
    {
        public static void RegisterServices(IServiceCollection services, HostBuilderContext hostContext)
        {
            services.AddHostedService<Worker>();
            services.AddSingleton<IGameRepository, GameRepository>();
            services.AddSingleton<IFvrzWebService, FvrzWebService>();
            
            var matchCenterSettings = new MatchCenterConfiguration();  
            new ConfigureFromConfigurationOptions<MatchCenterConfiguration>(hostContext.Configuration.GetSection("MatchCenterSettings")).Configure(matchCenterSettings);  
            services.AddSingleton(matchCenterSettings);
            
            var localConnectionTemplate = hostContext.Configuration.GetConnectionString("LocalSqliteConnection");
            var connectionString = new ConnectionStringConfiguration
            {
                LocalSqliteConnection = localConnectionTemplate.Replace("{PathToDbFile}", $"{Environment.GetEnvironmentVariable("HOME")}\\FootballAnalytics.db")
            };
            services.AddSingleton(connectionString);
        }
    }
}
