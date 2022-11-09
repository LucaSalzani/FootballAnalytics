using FootballAnalytics.Domain.Entities;

namespace FootballAnalytics.Application.Interfaces
{
    public interface IGameRepository // TODO: IGetAllGamesQueryHandlerRepository, IUpdateGamesWithLatestCommandHandlerRepository
    {
        Task<IEnumerable<Game>>GetAllGames();

        void UpsertGamesByGameNumber(IEnumerable<Game> games);
    }
}
