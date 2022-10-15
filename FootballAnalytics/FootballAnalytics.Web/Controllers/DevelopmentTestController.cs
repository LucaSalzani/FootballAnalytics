using FootballAnalytics.Application.Interfaces;
using FootballAnalytics.Infrastructure.Dtos;
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

        [HttpGet(Name = "games")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GameDto>))]
        public IActionResult GetAllGames()
        {
            return Ok(_gameRepository.GetAllGames().Select(GameDto.FromDomain));
        }
    }
}