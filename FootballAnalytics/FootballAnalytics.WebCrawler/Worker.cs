using System.Globalization;
using System.Net;
using System.Xml.XPath;
using FootballAnalytics.Domain.Entities;
using FootballAnalytics.Infrastructure;
using HtmlAgilityPack;

namespace FootballAnalytics.WebCrawler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHost _host;

        public Worker(ILogger<Worker> logger, IHost host)
        {
            _logger = logger;
            _host = host;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            const string url = "https://matchcenter.fvrz.ch/default.aspx?v=1417272&ls=21290&sg=60732&a=sp";
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var rootContainerNode = doc.DocumentNode.SelectSingleNode(XPathExpression.Compile(@"//*[@id=""ctl05_VereinMasterObjectID_ctl02_tbResultate""]"));

            var games = new List<ParsedGame>();
            var currentDate = string.Empty;
            foreach (var rootChild in rootContainerNode.ChildNodes.Where(cn => cn.Name is "div" or "a"))
            {
                if (rootChild.Name == "div")
                {
                    currentDate = rootChild.InnerText.Trim();
                    continue;
                }

                // Parse Game
                var parsedGame = new ParsedGame();
                parsedGame.Date = currentDate;
                var linkToGame = rootChild.GetAttributeValue("href", "notFound").Trim();
                parsedGame.LinkToGame = linkToGame;
                var gameContainerChildren = rootChild.FirstChild.Elements("div").ToList();

                var time = gameContainerChildren[0].InnerText.Trim();
                parsedGame.Time = time;
                var homeTeam = gameContainerChildren[1].Elements("div").First().InnerText.Trim();
                parsedGame.HomeTeam = homeTeam;
                var awayTeam = gameContainerChildren[1].Elements("div").Last().InnerText.Trim();
                parsedGame.AwayTeam = awayTeam;

                var hasScore = gameContainerChildren[2].Elements("div").Any();
                if (hasScore)
                {
                    var homeTeamScore = gameContainerChildren[2].Elements("div").First().InnerText.Trim();
                    parsedGame.HomeTeamScore = homeTeamScore;
                    var awayTeamScore = gameContainerChildren[2].Elements("div").Last().InnerText.Trim();
                    parsedGame.AwayTeamScore = awayTeamScore;
                }

                var gameNumber = gameContainerChildren[4].InnerText.Trim();
                parsedGame.GameNumber = gameNumber;

                games.Add(parsedGame);
            }

            var gameEntities = new List<Game>();
            foreach (var parsedGame in games)
            {
                var game = new Game();
                var date = DateOnly.Parse(parsedGame.Date);
                var time = TimeOnly.Parse(parsedGame.Time);
                game.GameDateBinary = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0).ToBinary();
                game.LinkToGame = $"https://matchcenter.fvrz.ch{parsedGame.LinkToGame}";
                game.HomeTeam = parsedGame.HomeTeam;
                game.AwayTeam = parsedGame.AwayTeam;
                if (parsedGame.HasScore)
                {
                    game.HomeTeamGoals = int.Parse(parsedGame.HomeTeamScore);
                    game.AwayTeamGoals = int.Parse(parsedGame.AwayTeamScore);
                }

                game.GameNumber = parsedGame.GameNumber;
                gameEntities.Add(game);
            }

            var repo = new GameRepository(); // TODO: Dependency injection, Update instead of insert, "Spielnummer XYZ" -> XYZ
            repo.StoreGames(gameEntities);

            await _host.StopAsync(stoppingToken);
        }
    }
}

internal struct ParsedGame
{
    public string Date;
    public string LinkToGame;
    public string Time;
    public string HomeTeam;
    public string AwayTeam;
    public string HomeTeamScore;
    public string AwayTeamScore;
    public string GameNumber;

    public bool HasScore => AwayTeamScore != null && HomeTeamScore != null;

}