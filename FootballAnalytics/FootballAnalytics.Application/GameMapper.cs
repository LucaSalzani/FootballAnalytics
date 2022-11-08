using System.Globalization;
using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Domain.Entities;
using FootballAnalytics.Domain.Model;

namespace FootballAnalytics.Application
{
    public class GameMapper : IGameMapper
    {
        private readonly string _matchCenterHostUrl;

        public GameMapper(string matchCenterHostUrl)
        {
            _matchCenterHostUrl = matchCenterHostUrl;
        }

        public IEnumerable<Game> MapFetchedGamesToEntities(IEnumerable<FetchedGame> fetchedGames)
        {
            return fetchedGames.Select(MapFetchedGameToEntity).ToList();
        }

        private Game MapFetchedGameToEntity(FetchedGame fetchedGame)
        {
            // TODO: Parsing error handling
            var game = new Game();
            var date = DateOnly.Parse(fetchedGame.Date, new CultureInfo("de-CH"));
            var time = TimeOnly.Parse(fetchedGame.Time, new CultureInfo("de-CH"));
            game.GameDateBinary =
                new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0).ToBinary();
            game.LinkToGame = $"{_matchCenterHostUrl}{fetchedGame.LinkToGame}";
            game.HomeTeam = fetchedGame.HomeTeam;
            game.AwayTeam = fetchedGame.AwayTeam;
            if (fetchedGame.HasScore)
            {
                game.HomeTeamGoals = int.Parse(fetchedGame.HomeTeamScore!);
                game.AwayTeamGoals = int.Parse(fetchedGame.AwayTeamScore!);
            }

            game.GameNumber = fetchedGame.GameNumber.Replace("Spielnummer ", string.Empty);
            return game;
        }
    }
}
