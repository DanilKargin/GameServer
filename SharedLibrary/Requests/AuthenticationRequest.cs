using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseImplement.Requests
{
	[System.Serializable]
	public class AuthenticationRequest
	{
		public string Login { get; set; }
		public string Password { get; set; }
	}
}
