using System;
using FootballAnalytics.Domain.Enums;

namespace FootballAnalytics.Domain.Entities
{
    public class Game
    {
        public string GameNumber { get; set; }
        public DateTime GameDate { get; set; }
        public Guid HomeTeamId { get; set; }
        public Guid AwayTeamId { get; set; }
        public MatchResult MatchResult { get; set; }
        public string LinkToGame { get; set; }
    }
}
