using Conqueror;

namespace FootballAnalytics.Application.UpdateGamesWithLatest;

[HttpCommand] // TODO: Add pragma in crawler command  
public sealed record UpdateGamesWithLatestCommand;

public interface IUpdateGamesWithLatestCommandHandler : ICommandHandler<UpdateGamesWithLatestCommand>
{
}