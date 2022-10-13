namespace FootballAnalytics.Domain.Model
{
    public class FetchedGame
    {
        public string Date { get; set; }
        public string LinkToGame { get; set; }
        public string Time { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string? HomeTeamScore { get; set; }
        public string? AwayTeamScore { get; set; }
        public string GameNumber { get; set; }

        public bool HasScore => AwayTeamScore != null && HomeTeamScore != null;
    }
}
