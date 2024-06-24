using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Responses
{
	public record VkProfileResponse(
		string? first_name = null,
		string? last_name = null,
		string? phone = null
		);
}
