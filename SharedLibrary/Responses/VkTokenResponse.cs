using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Responses
{
	public record class VkTokenResponse(
		string? access_token = null,
		int? expires_in = null,
		int? user_id = null,
		string? email = null
		);
}
