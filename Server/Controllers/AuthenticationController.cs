using DatabaseImplement.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Server.Services;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using System.ComponentModel;
using System.IO.Pipelines;

namespace Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthenticationController : ControllerBase
	{
		private readonly IAuthenticationService _authService;

		public AuthenticationController(IAuthenticationService authService)
		{
			_authService = authService;
		}

		[HttpGet("vkauth")]
		public IActionResult VkAuthentication(string code)
		{
			if (string.IsNullOrEmpty(code))
			{
				return BadRequest("Error receiving the code");
			}
			else
			{
				return Ok(_authService.LoginWithVK(code));
			}
		}
	}
}
