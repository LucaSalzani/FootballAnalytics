using System.Xml.XPath;
using FootballAnalytics.Domain.Model;
using HtmlAgilityPack;

namespace FootballAnalytics.Application
{
    public static class HtmlParser
    {
        public static IEnumerable<FetchedGame> ParseGamePlan(HtmlDocument document)
        {
            var rootContainerNode =
                document.DocumentNode.SelectSingleNode(
                    XPathExpression.Compile(@"//*[@id=""ctl05_VereinMasterObjectID_ctl02_tbResultate""]"));

            var currentDate = string.Empty;
            foreach (var rootChild in rootContainerNode.ChildNodes.Where(cn => cn.Name is "div" or "a"))
            {
                if (rootChild.Name == "div")
                {
                    currentDate = rootChild.InnerText.Trim();
                    continue;
                }

                var linkToGame = rootChild.GetAttributeValue("href", "notFound").Trim();
                var gameContainerChildren = rootChild.FirstChild.Elements("div").ToList();
                var time = gameContainerChildren[0].InnerText.Trim();
                var homeTeam = gameContainerChildren[1].Elements("div").First().InnerText.Trim();
                var awayTeam = gameContainerChildren[1].Elements("div").Last().InnerText.Trim();
                var hasScore = gameContainerChildren[2].Elements("div").Any();
                var gameNumber = gameContainerChildren[4].InnerText.Trim();

                var fetchedGame = new FetchedGame
                {
                    Date = currentDate,
                    LinkToGame = linkToGame,
                    Time = time,
                    HomeTeam = homeTeam,
                    AwayTeam = awayTeam,
                    GameNumber = gameNumber
                };

                if (hasScore)
                {
                    var homeTeamScore = gameContainerChildren[2].Elements("div").First().InnerText.Trim();
                    var awayTeamScore = gameContainerChildren[2].Elements("div").Last().InnerText.Trim();
                    fetchedGame.HomeTeamScore = homeTeamScore;
                    fetchedGame.AwayTeamScore = awayTeamScore;
                }

                yield return fetchedGame;
            }
        }
    }
}