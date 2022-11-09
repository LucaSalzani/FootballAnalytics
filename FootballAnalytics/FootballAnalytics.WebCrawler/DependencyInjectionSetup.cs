using FootballAnalytics.Application;
using FootballAnalytics.Infrastructure;

namespace FootballAnalytics.WebCrawler
{
    public static class DependencyInjectionSetup
    {
        public static void RegisterServices(IServiceCollection services, HostBuilderContext hostContext)
        {
            services.AddHostedService<Worker>();
            
            services.AddConfiguration(hostContext.Configuration, out var matchCenterConfiguration);
            services.AddInfrastructure();
            services.AddApplication(matchCenterConfiguration.MatchCenterHostUrl);

            services.ConfigureConqueror();
        }
    }
}
