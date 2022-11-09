using FootballAnalytics.Application;
using FootballAnalytics.Infrastructure;

namespace FootballAnalytics.Web
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddConquerorCQS();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddConfiguration(configuration, out var matchCenterConfiguration);
            services.AddInfrastructure();
            
            services.AddApplication(matchCenterConfiguration.MatchCenterHostUrl);

            services.ConfigureConqueror();
            return services;
        }
    }
}