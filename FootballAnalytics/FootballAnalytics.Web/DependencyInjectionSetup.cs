using FootballAnalytics.Application.GetAllGames;
using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Infrastructure;

namespace FootballAnalytics.Web
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddControllers().AddConquerorCQS();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddConquerorCQS().AddConquerorCQSTypesFromAssembly(typeof(GetAllGamesQueryHandler).Assembly); // TODO: DI per assembly
            
            services.AddTransient<IGameRepository, GameRepository>();
            services.ConfigureConqueror();
            return services;
        }
    }
}