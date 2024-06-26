using SharedLibrary.Responses;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace SharedLibrary.Models
{
	[DataContract]
	public class Player
    {
        public int Id { get; set; }
		public string Nickname { get; set; } = string.Empty;
		public int Currency { get; set; }
        public List<Car> Cars { get; set; }
		public List<PlayerRecord> Records { get; set; }


        public PlayerResponse GetPlayer()
		{
			Dictionary<string, int> dict = new Dictionary<string, int>();
			foreach (var item in Records)
			{
				dict.Add(item.RideType, item.Score);
			}
			return new PlayerResponse()
			{
				Id = Id,
				Nickname = Nickname,
				Currency = Currency,
				Cars = Cars.Select(x => x.Id).ToList(),
				Records = dict
			};
		}
    }
}
