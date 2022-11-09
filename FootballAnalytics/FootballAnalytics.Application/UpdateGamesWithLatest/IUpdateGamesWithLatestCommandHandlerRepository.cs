using FootballAnalytics.Domain.Entities;

namespace FootballAnalytics.Application.UpdateGamesWithLatest
{
    public interface IUpdateGamesWithLatestCommandHandlerRepository
    {
        Task UpsertGamesByGameNumber(IEnumerable<Game> games, CancellationToken cancellationToken);
    }
}
