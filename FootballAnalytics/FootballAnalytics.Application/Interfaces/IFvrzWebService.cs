using FootballAnalytics.Domain.Model;

namespace FootballAnalytics.Application.Interfaces
{
    public interface IFvrzWebService
    {
        IEnumerable<FetchedGame> FetchGames();
    }
}
