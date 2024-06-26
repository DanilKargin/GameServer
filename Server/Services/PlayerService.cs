using Microsoft.EntityFrameworkCore;
using Server.Models;
using SharedLibrary;
using SharedLibrary.Models;
using SharedLibrary.Requests.Player;
using SharedLibrary.Responses;

namespace Server.Services
{
	public class PlayerService
	{
		private readonly GameDBContext _context;

		public PlayerService(GameDBContext context)
		{
			_context = context;
		}

		public Player CreateNewPlayer()
		{
			var startCar = _context.Cars.FirstOrDefault(c => c.Name.Contains("Mini"));
			var player = new Player()
			{
				Currency = 1000,
				Cars = new List<Car>() { startCar },
				Records = new List<PlayerRecord>()
			};
			player.Nickname = "Username" + player.Id;
			_context.Add(player);
			_context.SaveChanges();

			return player;
		}
		public (bool success, string content) AddCar(int playerId, int carId)
		{
			var player = _context.Players.Include(p => p.Cars).FirstOrDefault(p => p.Id == playerId);
			if (player == null) { return (false, "Player not found"); }

			var car = _context.Cars.FirstOrDefault(c => c.Id == carId);
			if (car == null) { return (false, "Car not found"); }

			if(player.Currency >= car.Price)
			{
				player.Cars.Add(car);
				player.Currency -= car.Price;
				_context.SaveChanges();
				return (true, "");
			}
			return (false, "Insufficient funds");
		}
		public (bool success, string content) UpdateRecord(int playerId, string recordType, int score)
		{
			var player = _context.Players.Include(u => u.Records).FirstOrDefault(p => p.Id == playerId);
			if (player == null) { return (false, "Player not found"); }

			foreach(var item in player.Records)
			{
				if (item.RideType.Equals(recordType))
				{
					if(item.Score < score)
					{
						item.Score = score;
						item.RecordDate = DateTime.Now;
						_context.SaveChanges();
						return (true, "");
					}
					return (false, "Not enough points");
				}
			}
				var record = new PlayerRecord() { RecordDate = DateTime.Now, Score = score, Player = player, RideType = recordType };
				player.Records.Add(record);
				_context.Add(record);
				_context.SaveChanges();
			return (true, "");
		}
		public PlayerResponse EditPlayer(PlayerRequest request) 
		{
			var player = _context.Players.Include(p => p.Records).Include(p => p.Cars).FirstOrDefault(p => p.Id == request.Id);
			if (player == null) { return null; }
			if (!string.IsNullOrEmpty(request.Currency))
			{
				player.Currency = int.Parse(request.Currency);
			}
			if (!string.IsNullOrEmpty(request.Nickname))
			{
				player.Nickname = request.Nickname;
			}
			_context.SaveChanges();
			return player.GetPlayer();
		}
		public PlayerResponse GetPlayer(PlayerRequest request)
		{
			return _context.Players
				.Include(p => p.Records)
				.Include(p => p.Cars)
				.FirstOrDefault(p => p.Id == request.Id)?
				.GetPlayer();
		}
	}
}
