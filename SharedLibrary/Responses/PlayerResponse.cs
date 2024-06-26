using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Responses
{
	public class PlayerResponse
	{
		public int Id;
		public string Nickname { get; set; } = string.Empty;
		public int Currency { get; set; }
		public List<int> Cars { get; set; }
		public Dictionary<string, int> Records { get; set; }
	}
}
