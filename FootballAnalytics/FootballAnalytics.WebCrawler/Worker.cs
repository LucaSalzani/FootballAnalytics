using FootballAnalytics.Application.UpdateGamesWithLatest;

namespace FootballAnalytics.WebCrawler
{
    public class Worker : BackgroundService
    {
        private readonly IHost _host;
        private readonly IUpdateGamesWithLatestCommandHandler _updateGamesWithLatestCommandHandler;

        public Worker(IHost host, IUpdateGamesWithLatestCommandHandler updateGamesWithLatestCommandHandler)
        {
            _host = host;
            _updateGamesWithLatestCommandHandler = updateGamesWithLatestCommandHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _updateGamesWithLatestCommandHandler.ExecuteCommand(new UpdateGamesWithLatestCommand(), stoppingToken);
            await _host.StopAsync(stoppingToken);
        }
    }
}