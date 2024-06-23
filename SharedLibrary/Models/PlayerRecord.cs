using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
	[DataContract]
	public class PlayerRecord
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public string RideType { get; set; } = string.Empty;
        public int Score { get; set; }
        public Player Player { get; set; }
    }
}
