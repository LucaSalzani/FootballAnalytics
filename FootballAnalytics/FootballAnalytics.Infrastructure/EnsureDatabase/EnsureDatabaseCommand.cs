using Conqueror;

namespace FootballAnalytics.Infrastructure.EnsureDatabase;

#if DEBUG
[HttpCommand]
#endif
public sealed record EnsureDatabaseCommand;

public interface IEnsureDatabaseCommandHandler : ICommandHandler<EnsureDatabaseCommand>
{
}