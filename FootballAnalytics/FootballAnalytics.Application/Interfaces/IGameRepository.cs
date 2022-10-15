using FootballAnalytics.Domain.Entities;

namespace FootballAnalytics.Application.Interfaces
{
    public interface IGameRepository
    {
        IEnumerable<Game> GetAllGames();

        void UpsertGamesByGameNumber(IEnumerable<Game> games);
    }
}
