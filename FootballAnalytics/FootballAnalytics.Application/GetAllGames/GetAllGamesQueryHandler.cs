using FootballAnalytics.Application.Interfaces;

namespace FootballAnalytics.Application.GetAllGames;

public class GetAllGamesQueryHandler : IGetAllGamesQueryHandler
{
    private readonly IGameRepository _gameRepository;

    public GetAllGamesQueryHandler(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }
    
    public async Task<GetAllGamesQueryResponse> ExecuteQuery(GetAllGamesQuery query, CancellationToken cancellationToken)
    {
        var games = await _gameRepository.GetAllGames();
        return new GetAllGamesQueryResponse(games.Select(GameDto.FromDomain).ToList());
    }
}