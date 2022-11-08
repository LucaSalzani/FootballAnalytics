using FootballAnalytics.Application.Interfaces;

namespace FootballAnalytics.WebCrawler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHost _host;
        private readonly IGameRepository _gameRepository;
        private readonly IFvrzWebService _fvrzWebService;
        private readonly IGameMapper _gameMapper;

        public Worker(ILogger<Worker> logger,
            IHost host,
            IGameRepository gameRepository,
            IFvrzWebService fvrzWebService,
            IGameMapper gameMapper)
        {
            _logger = logger;
            _host = host;
            _gameRepository = gameRepository;
            _fvrzWebService = fvrzWebService;
            _gameMapper = gameMapper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Start fetching from web");
            var fetchedGames = _fvrzWebService.FetchGames();
            _logger.LogInformation("Finished fetching from web");

            var gameEntities = _gameMapper.MapFetchedGamesToEntities(fetchedGames);

            _gameRepository.UpsertGamesByGameNumber(gameEntities);
            _logger.LogInformation("Games stored in database");

            await _host.StopAsync(stoppingToken);
        }
    }
}