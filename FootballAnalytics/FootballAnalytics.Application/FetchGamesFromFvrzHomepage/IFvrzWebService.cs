namespace FootballAnalytics.Application.FetchGamesFromFvrzHomepage
{
    public interface IFvrzWebService
    {
        Task<IEnumerable<FetchedGame>> FetchGames(CancellationToken cancellationToken);
    }
}
