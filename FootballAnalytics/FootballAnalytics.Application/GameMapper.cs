using FootballAnalytics.Domain.Entities;
using FootballAnalytics.Domain.Model;

namespace FootballAnalytics.Application
{
    public static class GameMapper
    {
        public static IEnumerable<Game> MapFetchedGamesToEntities(IEnumerable<FetchedGame> fetchedGames)
        {
            return fetchedGames.Select(MapFetchedGameToEntity).ToList();
        }

        private static Game MapFetchedGameToEntity(FetchedGame fetchedGame)
        {
            var game = new Game();
            var date = DateOnly.Parse(fetchedGame.Date);
            var time = TimeOnly.Parse(fetchedGame.Time);
            game.GameDateBinary =
                new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0).ToBinary();
            game.LinkToGame = $"https://matchcenter.fvrz.ch{fetchedGame.LinkToGame}"; //  TODO: Create Link from config (pass in constructor, make not static)
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
