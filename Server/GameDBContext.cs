using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models;

namespace SharedLibrary
{
    public class GameDBContext : DbContext
	{
		public GameDBContext() : base()
		{

		}
		public GameDBContext(DbContextOptions<GameDBContext> options) : base(options)
		{

		}

		public DbSet<User> Users { get; set; }
		public DbSet<Player> Players { get; set; }
		public DbSet<Car> Cars { get; set; }
		public DbSet<PlayerRecord> PlayerRecords { get; set; }
	}
}
