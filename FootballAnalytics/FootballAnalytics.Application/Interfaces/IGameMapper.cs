using FootballAnalytics.Domain.Entities;
using FootballAnalytics.Domain.Model;

namespace FootballAnalytics.Application.Interfaces;

public interface IGameMapper
{
    IEnumerable<Game> MapFetchedGamesToEntities(IEnumerable<FetchedGame> fetchedGames);
}