using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Infrastructure;

namespace FootballAnalytics.WebCrawler
{
    public static class DependencyInjectionSetup
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddHostedService<Worker>();
            services.AddSingleton<IGameRepository, GameRepository>();
            services.AddSingleton<IFvrzWebService, FvrzWebService>();
        }
    }
}
