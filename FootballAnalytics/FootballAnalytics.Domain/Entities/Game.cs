using FootballAnalytics.Domain.Enums;

namespace FootballAnalytics.Domain.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string GameNumber { get; set; }
        public long GameDateBinary { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int? HomeTeamGoals { get; set; }
        public int? AwayTeamGoals { get; set; }
        public string LinkToGame { get; set; }
        public MatchResult MatchResult
        {
            get
            {
                if (HomeTeamGoals.HasValue && AwayTeamGoals.HasValue)
                {
                    return HomeTeamGoals > AwayTeamGoals
                        ? MatchResult.HomeTeamWin
                        : AwayTeamGoals > HomeTeamGoals ? MatchResult.AwayTeamWin : MatchResult.Draw;
                }

                return MatchResult.Pending;
            }
        }

        public DateTime GameDate
        {
            get => DateTime.FromBinary(GameDateBinary);
            set => GameDateBinary = value.ToBinary();
        }
    }
}
