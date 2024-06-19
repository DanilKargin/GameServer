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
        public User User { get; set; }

        public void Update(Player player)
        {
            if (player == null)
            {
                return;
            }
            Nickname = player.Nickname;
            Currency = player.Currency;
        }
    }
}
