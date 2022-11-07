using FootballAnalytics.Application;
using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Domain.Model;
using FootballAnalytics.Infrastructure.Configuration;
using HtmlAgilityPack;

namespace FootballAnalytics.Infrastructure
{
    public class FvrzWebService : IFvrzWebService
    {
        private readonly MatchCenterConfiguration _configuration;

        public FvrzWebService(MatchCenterConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<FetchedGame> FetchGames()
        {
            // TODO: Error Handling
            var url = _configuration.NapoliGamePlanUrl;
            var web = new HtmlWeb();
            var doc = web.Load(url);

            return HtmlParser.ParseGamePlan(doc);
        }
    }
}
