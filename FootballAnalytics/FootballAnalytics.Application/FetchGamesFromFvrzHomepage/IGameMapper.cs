using FootballAnalytics.Domain.Entities;

namespace FootballAnalytics.Application.FetchGamesFromFvrzHomepage;

public interface IGameMapper
{
    IEnumerable<Game> MapFetchedGamesToEntities(IEnumerable<FetchedGame> fetchedGames);
}