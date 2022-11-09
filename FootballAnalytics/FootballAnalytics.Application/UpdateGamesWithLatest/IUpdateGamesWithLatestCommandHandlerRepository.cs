using FootballAnalytics.Domain.Entities;

namespace FootballAnalytics.Application.UpdateGamesWithLatest
{
    public interface IUpdateGamesWithLatestCommandHandlerRepository
    {
        void UpsertGamesByGameNumber(IEnumerable<Game> games);
    }
}
