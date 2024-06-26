using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Models;
using Server.Services;
using SharedLibrary;
using SharedLibrary.Models;
using SharedLibrary.Requests.Player;
using SharedLibrary.Responses;
using System.Globalization;

namespace Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RecordController : ControllerBase
	{
		private readonly GameDBContext _context;
		private readonly PlayerService _playerService;
		public RecordController(GameDBContext context, PlayerService playerService)
		{
			_context = context;
			_playerService = playerService;
		}
		//[HttpPost("edit")]
		//public IActionResult EditPlayerRecord([FromBody] RecordRequest request)
		//{
		//	var user = int.Parse(User.FindFirst("id").Value);

		//	_playerService.UpdateRecord(_context.Users.Include(u => u.Player).FirstOrDefault(u => u.Id == user).Player.Id, request.RideType, request.Score);
		//	return Ok();
		//}

		[HttpGet("getday")]
		public List<RecordResponse> GetDayRecords()
		{
			var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
			List<RecordResponse> records = _context.PlayerRecords.Include(u => u.Player).Where(u => date < u.RecordDate).OrderByDescending(u => u.Score).Select(x => new RecordResponse()
			{
				Id = x.Id,
				PlayerName = x.Player.Nickname,
				RecordDate = x.RecordDate,
				RideType = x.RideType,
				Score = x.Score
			}).ToList();
			return records;
		}
		[HttpGet("getweek")]
		public List<RecordResponse> GetWeekRecords()
		{
			var date = DateTime.Today;

			while (date.DayOfWeek != DayOfWeek.Monday)
			{
				date = date.AddDays(-1);
			}

			var date2 = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
			List<RecordResponse> records = _context.PlayerRecords.Include(u => u.Player).Where(u => date < u.RecordDate).OrderByDescending(u => u.Score).Select(x => new RecordResponse()
			{
				Id = x.Id,
				PlayerName = x.Player.Nickname,
				RecordDate = x.RecordDate,
				RideType = x.RideType,
				Score = x.Score
			}).ToList();
			return records;
		}

		[HttpGet("getmonth")]
		public List<RecordResponse> GetMonthRecords()
		{
			var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1, 0, 0, 0);
			List<RecordResponse> records = _context.PlayerRecords.Include(u => u.Player).Where(u => date < u.RecordDate).OrderByDescending(u => u.Score).Select(x => new RecordResponse()
			{
				Id = x.Id,
				PlayerName = x.Player.Nickname,
				RecordDate = x.RecordDate,
				RideType = x.RideType,
				Score = x.Score
			}).ToList();
			return records;
		}
	}
}

