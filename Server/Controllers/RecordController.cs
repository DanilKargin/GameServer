using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using SharedLibrary;
using SharedLibrary.Models;
using System.Globalization;

namespace Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class RecordController : ControllerBase
	{
		private readonly ILogger<RecordController> _logger;
		private readonly GameDBContext _context;
		public RecordController(ILogger<RecordController> logger, GameDBContext context)
		{
			_logger = logger;
			_context = context;
		}

		[HttpGet("~/getdayrecords")]
		public List<RecordView> GetDayRecords()
		{
			var date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0);
			List<RecordView> records = _context.PlayerRecords.Include(u => u.Player).Where(u => date < u.RecordDate).OrderBy(u => u.Score).Select(x => new RecordView()
			{
				Id = x.Id,
				PlayerName = x.Player.Nickname,
				RecordDate = x.RecordDate,
				RideType = x.RideType,
				Score = x.Score
			}).ToList();
			return records;
		}
		[HttpGet("~/getweekrecords")]
		public List<RecordView> GetWeekRecords()
		{
			var date = DateTime.Today;

			while (date.DayOfWeek != DayOfWeek.Monday)
			{
				date = date.AddDays(-1);
			}

			var date2 = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
			List<RecordView> records = _context.PlayerRecords.Include(u => u.Player).Where(u => date < u.RecordDate).OrderBy(u => u.Score).Select(x => new RecordView()
			{
				Id = x.Id,
				PlayerName = x.Player.Nickname,
				RecordDate = x.RecordDate,
				RideType = x.RideType,
				Score = x.Score
			}).ToList();
			return records;
		}

		[HttpGet("~/getmonthrecords")]
		public List<RecordView> GetMonthRecords()
		{
			var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
			List<RecordView> records = _context.PlayerRecords.Include(u => u.Player).Where(u => date < u.RecordDate).OrderBy(u => u.Score).Select(x => new RecordView()
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

