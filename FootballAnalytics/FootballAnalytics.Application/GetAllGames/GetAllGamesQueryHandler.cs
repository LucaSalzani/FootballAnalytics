using FootballAnalytics.Application.Middlewares;

namespace FootballAnalytics.Application.GetAllGames;

internal sealed class GetAllGamesQueryHandler : IGetAllGamesQueryHandler, IConfigureQueryPipeline
{
    private readonly IGetAllGamesQueryHandlerRepository _gameRepository;

    public GetAllGamesQueryHandler(IGetAllGamesQueryHandlerRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }
    
    public async Task<GetAllGamesQueryResponse> ExecuteQuery(GetAllGamesQuery query, CancellationToken cancellationToken)
    {
        var games = await _gameRepository.GetAllGames();
        return new GetAllGamesQueryResponse(games.Select(GameDto.FromDomain).ToList());
    }
    
    public static void ConfigurePipeline(IQueryPipelineBuilder pipeline)
    {
        pipeline.UseDefault();
    }
}