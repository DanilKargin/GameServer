using DatabaseImplement.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
		[HttpGet("login")]
		public AuthenticationResponse Login(string _login, string _password)
		{
			var login = _authService.Login(_login, _password);
			if (!login.success)
			{
				return new AuthenticationResponse() { Token = null, ErrorMessage = login.token };
			}
			return new AuthenticationResponse() { Token = login.token };
		}
		[HttpGet("register")]
		public AuthenticationResponse Register(string login, string password)
		{
			var register = _authService.Register(login, password);
			if (!register.success)
			{
				return new AuthenticationResponse() { Token = null, ErrorMessage = register.content};
			}
			else
			{
				return new AuthenticationResponse() { Token = _authService.Login(login, password).token };
			}

		}

		[HttpGet("vkauth")]
		public AuthenticationResponse VkAuthentication(string access_token, string user_id, string email)
		{
			if (string.IsNullOrEmpty(access_token))
			{
				return new AuthenticationResponse() { Token = null, ErrorMessage = "Invalid token!" };
			}
			else
			{
				return new AuthenticationResponse() { Token = _authService.LoginWithVK(access_token, int.Parse(user_id), email).Result.content};
			}
		}
	}
}
