using FootballAnalytics.Application;
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
            
            var matchCenterConfiguration = RegisterMatchCenterConfiguration(services, hostContext);
            RegisterConnectionStringConfiguration(services, hostContext);
            
            services.AddSingleton<IGameRepository, GameRepository>();
            services.AddSingleton<IFvrzWebService, FvrzWebService>();
            services.AddSingleton<IGameMapper, GameMapper>(_ => new GameMapper(matchCenterConfiguration.MatchCenterHostUrl));
        }

        private static void RegisterConnectionStringConfiguration(IServiceCollection services, HostBuilderContext hostContext)
        {
            var connectionString = ConnectionStringConfiguration.FromConfiguration(hostContext.Configuration);
            services.AddSingleton(connectionString);
        }

        private static MatchCenterConfiguration RegisterMatchCenterConfiguration(IServiceCollection services, HostBuilderContext hostContext)
        {
            var matchCenterSettings = new MatchCenterConfiguration();
            new ConfigureFromConfigurationOptions<MatchCenterConfiguration>(
                hostContext.Configuration.GetSection("MatchCenterSettings")).Configure(matchCenterSettings);
            services.AddSingleton(matchCenterSettings);
            return matchCenterSettings;
        }
    }
}
