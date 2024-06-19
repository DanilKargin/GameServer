using SharedLibrary.Models;

namespace Server.Models
{
	public class PlayerView
	{
		public int Id { get; set; }
		public string Nickname { get; set; }
		public int Currency { get; set; }
		public List<Car> Cars { get; set; }
		public List<PlayerRecord> PlayerRecords { get; set; }
	}
}
