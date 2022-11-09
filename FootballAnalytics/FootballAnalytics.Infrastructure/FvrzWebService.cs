using FootballAnalytics.Application;
using FootballAnalytics.Application.FetchGamesFromFvrzHomepage;
using FootballAnalytics.Application.UpdateGamesWithLatest;
using FootballAnalytics.Infrastructure.Configuration;
using HtmlAgilityPack;

namespace FootballAnalytics.Infrastructure
{
    public sealed class FvrzWebService : IFvrzWebService
    {
        private readonly MatchCenterConfiguration _configuration;

        public FvrzWebService(MatchCenterConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<FetchedGame>> FetchGames(CancellationToken cancellationToken)
        {
            // TODO: Error Handling
            var url = _configuration.NapoliGamePlanUrl;
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url, cancellationToken);

            return HtmlParser.ParseGamePlan(doc);
        }
    }
}
