using FootballAnalytics.Application;
using FootballAnalytics.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalytics.Web.Controllers
{
    [ApiController]
    [Route("devtest")]
    public class DevelopmentTestController : ControllerBase
    {
        private readonly IGameRepository _gameRepository;

        public DevelopmentTestController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        // [HttpGet("games")]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GameDto>))]
        // public async Task<IActionResult> GetAllGames()
        // {
        //     var allGames = await _gameRepository.GetAllGames();
        //     return Ok(allGames.Select(GameDto.FromDomain));
        // }
        
        [HttpGet("ping")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public IActionResult GetPing()
        {
            return Ok("It works automated");
        }

        [HttpGet("worstplace")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        public async Task<IActionResult> CalculateWorstPlaceForSpecificTeam()
        {
            var games = await _gameRepository.GetAllGames();
            var teamId = "Napoli Club Zurigo Partenopea 1";
            var calc = new LeagueCalculator(games);

            var rankings = new List<Ranking>();
            for (var i = 0; i < 1500; i++)
            {
                rankings.Add(calc.CalculateWorstPlaceForSpecificTeam(teamId));
            }

            var worst = rankings.Select(r => r.Rank + r.Teams.Count - 1).MaxBy(x => x);

            return Ok(worst);
        }
    }
}