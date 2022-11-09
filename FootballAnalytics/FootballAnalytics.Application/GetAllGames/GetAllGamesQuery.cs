using Conqueror;

namespace FootballAnalytics.Application.GetAllGames;

[HttpQuery] // TODO: Add pragma in crawler command  
public sealed record GetAllGamesQuery;

public sealed record GetAllGamesQueryResponse(IReadOnlyCollection<GameDto> Games);

public interface IGetAllGamesQueryHandler : IQueryHandler<GetAllGamesQuery, GetAllGamesQueryResponse>
{
}