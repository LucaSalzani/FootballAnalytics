using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Infrastructure;

namespace FootballAnalytics.Web
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddTransient<IGameRepository, GameRepository>();
            return services;
        }
    }
}
