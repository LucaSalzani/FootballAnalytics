using FootballAnalytics.Application.FetchGamesFromFvrzHomepage;
using FootballAnalytics.Application.Middlewares;

namespace FootballAnalytics.Application.UpdateGamesWithLatest;

internal sealed class UpdateGamesWithLatestCommandHandler : IUpdateGamesWithLatestCommandHandler, IConfigureCommandPipeline
{
    private readonly IUpdateGamesWithLatestCommandHandlerRepository _gameRepository;
    private readonly IFetchGamesFromFvrzHomepageQueryHandler _fetchGamesFromFvrzHomepageQueryHandler;

    public UpdateGamesWithLatestCommandHandler(IUpdateGamesWithLatestCommandHandlerRepository gameRepository,
        IFetchGamesFromFvrzHomepageQueryHandler fetchGamesFromFvrzHomepageQueryHandler)
    {
        _gameRepository = gameRepository;
        _fetchGamesFromFvrzHomepageQueryHandler = fetchGamesFromFvrzHomepageQueryHandler;
    }
    
    public async Task ExecuteCommand(UpdateGamesWithLatestCommand command, CancellationToken cancellationToken)
    {
        var fetchGamesFromFvrzHomepageQueryResponse =
            await _fetchGamesFromFvrzHomepageQueryHandler.ExecuteQuery(new(), cancellationToken);

        _gameRepository.UpsertGamesByGameNumber(fetchGamesFromFvrzHomepageQueryResponse.Games);
    }
    
    // ReSharper disable once UnusedMember.Global
    public static void ConfigurePipeline(ICommandPipelineBuilder pipeline)
    {
        pipeline.UseDefault();
    }
}