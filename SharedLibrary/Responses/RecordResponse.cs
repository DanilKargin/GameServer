using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Responses
{
	public class RecordResponse
	{
		public int Id { get; set; }
		public DateTime RecordDate { get; set; }
		public string RideType { get; set; } = string.Empty;
		public int Score { get; set; }
		public string PlayerName { get; set; }
	}
}
