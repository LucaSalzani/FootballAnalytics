using FootballAnalytics.Domain.Entities;

namespace FootballAnalytics.Application.Interfaces
{
    public interface IGameRepository
    {
        void StoreGames(IEnumerable<Game> games);
    }
}
