using FootballAnalytics.Application;
using FootballAnalytics.Application.Interfaces;

namespace FootballAnalytics.WebCrawler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHost _host;
        private readonly IGameRepository _gameRepository;
        private readonly IFvrzWebService _fvrzWebService;

        public Worker(ILogger<Worker> logger, IHost host, IGameRepository gameRepository, IFvrzWebService fvrzWebService)
        {
            _logger = logger;
            _host = host;
            _gameRepository = gameRepository;
            _fvrzWebService = fvrzWebService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var fetchedGames = _fvrzWebService.FetchGames();
            _logger.LogInformation("Finished fetching from web");

            var gameEntities = GameMapper.MapFetchedGamesToEntities(fetchedGames);

            _gameRepository.StoreGames(gameEntities);
            _logger.LogInformation("Stored in Database");

            await _host.StopAsync(stoppingToken);
        }
    }
}

