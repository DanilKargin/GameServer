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
using static System.Net.WebRequestMethods;

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

		public async Task<(bool success, string content)> LoginWithVK(string code)
		{
			string url = "https://oauth.vk.com/access_token?client_id=" + _settings.AppId + "&client_secret=" + _settings.SecretKey + "&redirect_uri=" + _settings.RedirectUri + "&code=" + code;
			string urlGetProfileInfo = "https://api.vk.com/method/account.getProfileInfo";

			HttpClient httpClient = new HttpClient();
			var result = await httpClient.GetFromJsonAsync<VkTokenResponse>(url);
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.access_token);

			var profile_info = await httpClient.GetFromJsonAsync<VkProfileResponse>(urlGetProfileInfo);
			string nickname = profile_info.last_name + " " + profile_info.first_name;

			if(result == null)
			{
				return (false, "Vk token invalid");
			}
			if(!Register(result.email, result.user_id.ToString(), nickname).success)
			{
				return Login(result.email, result.user_id.ToString());
			}
			return (false, "?");
			
		}

		public (bool success, string content) Register(string login, string password, string nickname)
		{
			if (_context.Users.Include(u => u.Player).Any(u => u.Login == login))
				return (false, "Login not available");

			var player = _playerService.CreateNewPlayer(nickname);
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
		public Task<(bool success, string content)> LoginWithVK(string code);
		(bool success, string content) Register(string login, string password, string nickname);
		(bool success, string token) Login(string login, string password);
	}
}
