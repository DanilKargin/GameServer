using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Services;
using SharedLibrary;
using SharedLibrary.Models;
using SharedLibrary.Requests.Player;
using SharedLibrary.Responses;

namespace Server.Controllers
{
    [Authorize]
    [ApiController]
	[Route("[controller]")]
	public class PlayerController : ControllerBase
	{
		private readonly PlayerService _playerService;
		public PlayerController(GameDBContext context, PlayerService playerService)
		{
			_playerService = playerService;
		}
		[HttpGet("get")]
		public PlayerResponse Get() 
		{
			var playerId = int.Parse(User.FindFirst("playerId").Value);
			return _playerService.GetPlayer(new PlayerRequest() { Id = playerId });
		}
		[HttpPost("addcar")]
		public IActionResult EditCarList(CarRequest request)
		{
			var playerId = int.Parse(User.FindFirst("playerId").Value);
			var (success, content) = _playerService.AddCar(playerId, request.Id);
			if (success)
			{
				return Ok();
			}
			else
			{
				return BadRequest(content);
			}

		}
		[HttpGet("edit")]
		public PlayerResponse Edit(string nickname, string cash)
		{
			int Id = int.Parse(User.FindFirst("playerId").Value);
			return _playerService.EditPlayer(new PlayerRequest() { Id = Id, Currency = cash, Nickname = nickname });
		}
	}
}
