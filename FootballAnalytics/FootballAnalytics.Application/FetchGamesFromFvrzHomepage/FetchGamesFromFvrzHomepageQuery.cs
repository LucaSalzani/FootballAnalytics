using FootballAnalytics.Domain.Entities;

namespace FootballAnalytics.Application.FetchGamesFromFvrzHomepage;

[HttpQuery]
public sealed record FetchGamesFromFvrzHomepageQuery;

public sealed record FetchGamesFromFvrzHomepageQueryResponse(IReadOnlyCollection<Game> Games);

public interface IFetchGamesFromFvrzHomepageQueryHandler : IQueryHandler<FetchGamesFromFvrzHomepageQuery, FetchGamesFromFvrzHomepageQueryResponse>
{
}