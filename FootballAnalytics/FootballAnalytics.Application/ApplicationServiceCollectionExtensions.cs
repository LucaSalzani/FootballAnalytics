using FootballAnalytics.Application.FetchGamesFromFvrzHomepage;
using FootballAnalytics.Application.UpdateGamesWithLatest;
using Microsoft.Extensions.DependencyInjection;

namespace FootballAnalytics.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, string matchCenterHostUrl)
    {
        services.AddSingleton<IGameMapper, GameMapper>(_ => new GameMapper(matchCenterHostUrl));
        services.AddConquerorCQS().AddConquerorCQSTypesFromExecutingAssembly();

        return services;
    }
}