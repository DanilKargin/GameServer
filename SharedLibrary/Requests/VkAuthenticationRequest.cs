using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Requests
{
	[System.Serializable]
	public class VkAuthenticationRequest
	{
		public string access_token {  get; set; }
		public int user_id { get; set; }
		public string email { get; set; }
	}
}
