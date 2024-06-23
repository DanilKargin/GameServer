using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Services;
using SharedLibrary;
using SharedLibrary.Models;
using SharedLibrary.Requests.Player;

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
		[HttpGet]
		public IActionResult Get() 
		{
			var playerId = int.Parse(User.FindFirst("playerId").Value);
			var player = _playerService.GetPlayer(new PlayerRequest() { Id = playerId });
			return Ok(_playerService.GetPlayer(new PlayerRequest() { Id = playerId }));
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
		[HttpPost("edit")]
		public IActionResult Edit(PlayerRequest request)
		{
			request.Id = int.Parse(User.FindFirst("playerId").Value);
			return Ok(_playerService.EditPlayer(request));
		}
	}
}
