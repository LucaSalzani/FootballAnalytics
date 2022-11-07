using FootballAnalytics.Domain.Entities;
using FootballAnalytics.Domain.Enums;

namespace FootballAnalytics.Application
{
    public class LeagueCalculator
    {
        private readonly List<Game> _currentLeagueGameState;

        public LeagueCalculator(IEnumerable<Game> currentLeagueGameState)
        {
            _currentLeagueGameState = currentLeagueGameState.ToList();
        }

        public Ranking CalculateWorstPlaceForSpecificTeam(string teamIdentifier)
        {
            var currentTable = CalculateCurrentTable();

            // DO CALC
            foreach (var pendingGame in _currentLeagueGameState.Where(g => g.MatchResult == MatchResult.Pending))
            {
                if (pendingGame.HomeTeam == teamIdentifier)
                {
                    currentTable = WriteGameResultInTable(currentTable, pendingGame, 0, 3);
                } 
                else if (pendingGame.AwayTeam == teamIdentifier)
                {
                    currentTable = WriteGameResultInTable(currentTable, pendingGame, 3, 0);
                }
                else
                {
                    var random = new Random();
                    var isDraw = random.NextDouble() < 0.15;
                    if (isDraw)
                    {
                        currentTable = WriteGameResultInTable(currentTable, pendingGame, 1, 1);
                    }
                    else
                    {
                        var homeTeamWin = random.NextDouble() > 0.5;
                        currentTable = WriteGameResultInTable(currentTable, pendingGame, homeTeamWin ? 3 : 0,
                            homeTeamWin ? 0 : 3);
                    }
                }
            }

            var points = currentTable.Values.Distinct().OrderByDescending(x => x);
            var rankings = new List<Ranking>();
            var currentRank = 1;
            foreach (var point in points)
            {
                var ranking = new Ranking
                {
                    Rank = currentRank,
                    Points = point,
                    Teams = new List<string>(currentTable.Where(t => t.Value == point).Select(t => t.Key))
                };

                rankings.Add(ranking);

                currentRank++;
            }

            return rankings.First(r => r.Teams.Contains(teamIdentifier));
        }

        private Dictionary<string, int> CalculateCurrentTable()
        {
            var currentTable = new Dictionary<string, int>();
            foreach (var game in _currentLeagueGameState)
            {
                var homeTeamPoints = GetHomeTeamPoints(game);
                var awayTeamPoints = GetAwayTeamPoints(game);
                currentTable = WriteGameResultInTable(currentTable, game, homeTeamPoints, awayTeamPoints);
            }

            return currentTable;
        }

        private Dictionary<string, int> WriteGameResultInTable(Dictionary<string, int> table, Game game, int homeTeamPoints, int awayTeamPoints)
        {
            if (table.ContainsKey(game.HomeTeam))
            {
                table[game.HomeTeam] += homeTeamPoints;
            }
            else
            {
                table.Add(game.HomeTeam, homeTeamPoints);
            }
            if (table.ContainsKey(game.AwayTeam))
            {
                table[game.AwayTeam] += awayTeamPoints;
            }
            else
            {
                table.Add(game.AwayTeam, awayTeamPoints);
            }

            return table;
        }

        private int GetHomeTeamPoints(Game game)
        {
            return game.MatchResult switch
            {
                MatchResult.HomeTeamWin => 3,
                MatchResult.Draw => 1,
                _ => 0
            };
        }

        private int GetAwayTeamPoints(Game game)
        {
            return game.MatchResult switch
            {
                MatchResult.AwayTeamWin => 3,
                MatchResult.Draw => 1,
                _ => 0
            };
        }
    }

    public class Ranking
    {
        public int Rank { get; set; }

        public int Points { get; set; }

        public List<string> Teams { get; set; }
    }
}
