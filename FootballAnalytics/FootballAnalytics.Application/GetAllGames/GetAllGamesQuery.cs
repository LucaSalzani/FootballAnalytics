using Conqueror;

namespace FootballAnalytics.Application.GetAllGames;

[HttpQuery]
public sealed record GetAllGamesQuery;

public sealed record GetAllGamesQueryResponse(IReadOnlyCollection<GameDto> Games);

public interface IGetAllGamesQueryHandler : IQueryHandler<GetAllGamesQuery, GetAllGamesQueryResponse>
{
}