using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server.Helpers;
using Server.Models;
using SharedLibrary;
using SharedLibrary.Models;
using SharedLibrary.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace Server.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly SettingsOption _settings;
		private readonly GameDBContext _context;
		private readonly PlayerService _playerService;
		
		public AuthenticationService(SettingsOption settings, GameDBContext context, PlayerService playerService)
		{
			_settings = settings;
			_context = context;
			_playerService = playerService;
		}

		public async Task<(bool success, string content)> LoginWithVK(string code, int user_id, string email)
		{
			string urlGetProfileInfo = "https://api.vk.com/method/account.getProfileInfo?v=5.131";

			HttpClient httpClient = new HttpClient();
			
			//httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", code);

			//var profile_info = await httpClient.GetFromJsonAsync<Response>(urlGetProfileInfo);
			//string nickname = profile_info.response.last_name + " " + profile_info.response.first_name;

			if(!Register(email, user_id.ToString()).success)
			{
				return Login(email, user_id.ToString());
			}
			return (false, "?");
			
		}

		public (bool success, string content) Register(string login, string password)
		{
			if (_context.Users.Include(u => u.Player).Any(u => u.Login == login))
				return (false, "Login not available");

			var player = _playerService.CreateNewPlayer();
			var user = new User { Login = login, PasswordHash = password, Player = player };
			user.ProvideSaltAndHash();

			_context.Add(user);
			_context.SaveChanges();

			return (true, "");
		}

		public (bool success, string token) Login(string login, string password)
		{
			var user = _context.Users.Include(u => u.Player).SingleOrDefault(u => u.Login == login);
			if (user == null)
				return (false, "Invalid username");

			if (user.PasswordHash != AuthenticationHelpers.ComputeHash(password, user.Salt))
				return (false, "Invalid password");

			return (true, GenerateJwtToken(AssembleClaimsIdentity(user)));
		}

		private ClaimsIdentity AssembleClaimsIdentity(User user)
		{
			var subject = new ClaimsIdentity(new[]
			{
				new Claim("id", user.Id.ToString()),
				new Claim("playerId", user.Player.Id.ToString())
			});
			return subject;
		}
		
		private string GenerateJwtToken(ClaimsIdentity subject)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_settings.BearerKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = subject,
				Expires = DateTime.Now.AddYears(10),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
	public interface IAuthenticationService
	{
		public Task<(bool success, string content)> LoginWithVK(string code, int user_id, string email);
		(bool success, string content) Register(string login, string password);
		(bool success, string token) Login(string login, string password);
	}
}
