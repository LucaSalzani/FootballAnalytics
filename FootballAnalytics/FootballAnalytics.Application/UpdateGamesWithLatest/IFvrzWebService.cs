using FootballAnalytics.Domain.Model;

namespace FootballAnalytics.Application.UpdateGamesWithLatest
{
    public interface IFvrzWebService
    {
        Task<IEnumerable<FetchedGame>> FetchGames();
    }
}
