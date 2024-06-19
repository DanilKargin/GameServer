using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SharedLibrary;
using SharedLibrary.Models;
using SharedLibrary.Requests;

namespace Server.Controllers
{
	[Authorize]
    [ApiController]
	[Route("[controller]")]
	public class PlayerController : ControllerBase
	{
		private readonly ILogger<PlayerController> _logger;
		private readonly GameDBContext _context;
		public PlayerController(ILogger<PlayerController> logger, GameDBContext context)
		{
			_logger = logger;
			_context = context;
		}

		[HttpPost("{id}")]
		public IActionResult Edit([FromRoute] int id, [FromBody] CreatePlayerRequest request)
		{
			var playerIsAvailable = JsonConvert.DeserializeObject<List<int>>(User.FindFirst("players").Value);
			if (!playerIsAvailable.Contains(id))
				return Unauthorized();
            var player = _context.Players.First(x => x.Id == id);
			player.Nickname = request.Nickname;
			_context.SaveChanges();
			return Ok();
		}
		[HttpPost]
		public Player Create(CreatePlayerRequest request)
		{
			var userId = int.Parse(User.FindFirst("id").Value);

			var user = _context.Users.Include(u => u.Players).First(u => u.Id == userId);
			var startCar = _context.Cars.First(c => c.Name.Contains("Starter"));
			var player = new Player()
			{
				Nickname = request.Nickname,
				Currency = 1000,
				User = user,
				Cars = new List<Car>() { startCar }
			};
			_context.Add(player);
			_context.SaveChanges();

			player.User = null;

			return player;
		}
	}
}
