using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Server.Helpers;
using Server.Models;
using SharedLibrary;
using SharedLibrary.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Services
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly Settings _settings;
		private readonly GameDBContext _context;
		
		public AuthenticationService(Settings settings, GameDBContext context)
		{
			_settings = settings;
			_context = context;
		}

		public (bool success, string content) Register(string login, string password)
		{
			if (_context.Users.Any(u => u.Login == login))
				return (false, "Login not available");

			var user = new User { Login = login, PasswordHash = password };
			user.ProvideSaltAndHash();

			_context.Add(user);
			_context.SaveChanges();

			return (true, "");
		}

		public (bool success, string token) Login(string login, string password)
		{
			var user = _context.Users.Include(u => u.Players).SingleOrDefault(u => u.Login == login);
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
				new Claim("players", JsonConvert.SerializeObject(user.Players.Select(h => h.Id)))
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
		(bool success, string content) Register(string login, string password);
		(bool success, string token) Login(string login, string password);
	}
}
