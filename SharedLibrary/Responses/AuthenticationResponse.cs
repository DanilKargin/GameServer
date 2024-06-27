using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Responses
{
	public class AuthenticationResponse
	{
		public string Token { get; set; } = string.Empty;
		public string ErrorMessage { get; set; } = string.Empty;
	}
}
