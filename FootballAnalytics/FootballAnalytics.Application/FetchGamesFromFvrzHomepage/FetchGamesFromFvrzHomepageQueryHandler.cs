using FootballAnalytics.Application.Middlewares;

namespace FootballAnalytics.Application.FetchGamesFromFvrzHomepage;

public sealed class FetchGamesFromFvrzHomepageQueryHandler : IFetchGamesFromFvrzHomepageQueryHandler, IConfigureQueryPipeline
{
    private readonly IFvrzWebService _fvrzWebService;
    private readonly IGameMapper _gameMapper;

    public FetchGamesFromFvrzHomepageQueryHandler(IFvrzWebService fvrzWebService, IGameMapper gameMapper)
    {
        _fvrzWebService = fvrzWebService;
        _gameMapper = gameMapper;
    }
    
    public async Task<FetchGamesFromFvrzHomepageQueryResponse> ExecuteQuery(FetchGamesFromFvrzHomepageQuery query, CancellationToken cancellationToken)
    {
        var fetchedGames = await _fvrzWebService.FetchGames(cancellationToken);
        var gameEntities = _gameMapper.MapFetchedGamesToEntities(fetchedGames);
        
        return new FetchGamesFromFvrzHomepageQueryResponse(gameEntities.ToList());
    }
    
    public static void ConfigurePipeline(IQueryPipelineBuilder pipeline)
    {
        pipeline.UseDefault();
    }
}