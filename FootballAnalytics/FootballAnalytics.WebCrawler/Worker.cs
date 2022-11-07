using FootballAnalytics.Application;
using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Infrastructure.Configuration;

namespace FootballAnalytics.WebCrawler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHost _host;
        private readonly IGameRepository _gameRepository;
        private readonly IFvrzWebService _fvrzWebService;
        private readonly MatchCenterConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IHost host, IGameRepository gameRepository, IFvrzWebService fvrzWebService, MatchCenterConfiguration configuration)
        {
            _logger = logger;
            _host = host;
            _gameRepository = gameRepository;
            _fvrzWebService = fvrzWebService;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var fetchedGames = _fvrzWebService.FetchGames();
            _logger.LogInformation("Finished fetching from web");

            var gameMapper = new GameMapper(_configuration.MatchCenterHostUrl);
            var gameEntities = gameMapper.MapFetchedGamesToEntities(fetchedGames);

            _gameRepository.UpsertGamesByGameNumber(gameEntities);
            _logger.LogInformation("Stored in Database");

            await _host.StopAsync(stoppingToken);
        }
    }
}

