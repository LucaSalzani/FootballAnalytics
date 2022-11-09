using Conqueror;

namespace FootballAnalytics.Application.UpdateGamesWithLatest;

#if DEBUG
[HttpCommand]
#endif
public sealed record UpdateGamesWithLatestCommand;

public interface IUpdateGamesWithLatestCommandHandler : ICommandHandler<UpdateGamesWithLatestCommand>
{
}