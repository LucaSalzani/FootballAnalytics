using FootballAnalytics.Domain.Entities;

namespace FootballAnalytics.Application.GetAllGames;

public interface IGetAllGamesQueryHandlerRepository
{
    Task<IEnumerable<Game>> GetAllGames(CancellationToken cancellationToken);
}