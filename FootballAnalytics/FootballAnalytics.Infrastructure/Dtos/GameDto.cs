using FootballAnalytics.Domain.Entities;
using FootballAnalytics.Domain.Enums;

namespace FootballAnalytics.Infrastructure.Dtos
{
    public class GameDto
    {
        public string GameNumber { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }
        public string LinkToGame { get; set; }
        public MatchResult MatchResult { get; set; } // TODO:  Human readable format?
        public DateTime GameDate { get; set; }

        public static GameDto FromDomain(Game game)
        {
            return new GameDto
            {
                GameNumber = game.GameNumber,
                HomeTeam = game.HomeTeam,
                AwayTeam = game.AwayTeam,
                HomeTeamGoals = game.HomeTeamGoals,
                AwayTeamGoals = game.AwayTeamGoals,
                LinkToGame = game.LinkToGame,
                MatchResult = game.MatchResult,
                GameDate = game.GameDate
            };
        }

        public static Game ToDomain(GameDto dto, int id)
        {
            return new Game
            {
                GameNumber = dto.GameNumber,
                HomeTeam = dto.HomeTeam,
                AwayTeam = dto.AwayTeam,
                HomeTeamGoals = dto.HomeTeamGoals,
                AwayTeamGoals = dto.AwayTeamGoals,
                LinkToGame = dto.LinkToGame,
                GameDate = dto.GameDate,
                Id = id
            };
        }
    }
}
