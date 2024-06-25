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
		string? maiden_name = null,
		int? sex = null,
		int? relation = null,
		object? relation_partner = null,
		int? relation_pending = null,
		object[]? relation_request = null,
		string? bdate = null,
		int? bdate_visibility = null,
		string? home_town = null,
		object? country = null,
		object? city = null,
		object? name_request = null,
		string? status = null,
		string? phone = null
		);
}
