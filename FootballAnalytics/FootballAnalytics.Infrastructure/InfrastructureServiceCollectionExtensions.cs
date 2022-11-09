using FootballAnalytics.Application.FetchGamesFromFvrzHomepage;
using FootballAnalytics.Application.GetAllGames;
using FootballAnalytics.Application.UpdateGamesWithLatest;
using FootballAnalytics.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FootballAnalytics.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddConquerorCQS().AddConquerorCQSTypesFromExecutingAssembly();
        
        services.AddSingleton<IGetAllGamesQueryHandlerRepository, GameRepository>();
        services.AddSingleton<IUpdateGamesWithLatestCommandHandlerRepository, GameRepository>();
        services.AddSingleton<GameRepository>();
        services.AddSingleton<IFvrzWebService, FvrzWebService>();
        return services;
    }

    public static IServiceCollection AddConfiguration(
        this IServiceCollection services,
        IConfiguration configuration,
        out MatchCenterConfiguration matchCenterConfiguration)
    {
        var connectionString = ConnectionStringConfiguration.FromConfiguration(configuration);
        services.AddSingleton(connectionString);

        matchCenterConfiguration = new MatchCenterConfiguration();
        configuration.GetSection("MatchCenterSettings").Bind(matchCenterConfiguration);
        services.AddSingleton(matchCenterConfiguration);

        return services;
    }
}