using Conqueror;
using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Application.Middlewares;
using Microsoft.Extensions.Logging;

namespace FootballAnalytics.Application.UpdateGamesWithLatest;

// TODO: Visibility In whole project
public class UpdateGamesWithLatestCommandHandler : IUpdateGamesWithLatestCommandHandler, IConfigureCommandPipeline
{
    private readonly ILogger<UpdateGamesWithLatestCommandHandler> _logger;
    private readonly IGameRepository _gameRepository;
    private readonly IFvrzWebService _fvrzWebService;
    private readonly IGameMapper _gameMapper;

    public UpdateGamesWithLatestCommandHandler(ILogger<UpdateGamesWithLatestCommandHandler> logger,
        IGameRepository gameRepository,
        IFvrzWebService fvrzWebService,
        IGameMapper gameMapper)
    {
        _logger = logger;
        _gameRepository = gameRepository;
        _fvrzWebService = fvrzWebService;
        _gameMapper = gameMapper;
    }
    
    public async Task ExecuteCommand(UpdateGamesWithLatestCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Start fetching from web");
        var fetchedGames = await _fvrzWebService.FetchGames(); // TODO Add query to fetch games
        _logger.LogInformation("Finished fetching from web");

        var gameEntities = _gameMapper.MapFetchedGamesToEntities(fetchedGames);

        _gameRepository.UpsertGamesByGameNumber(gameEntities);
        _logger.LogInformation("Games stored in database");
    }
    
    // ReSharper disable once UnusedMember.Global
    public static void ConfigurePipeline(ICommandPipelineBuilder pipeline)
    {
        pipeline.UseLogging();
    }
}