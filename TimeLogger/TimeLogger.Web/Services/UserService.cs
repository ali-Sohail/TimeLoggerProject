using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TimeLogger.Web.Helpers;
using TimeLogger.Web.Models;

namespace TimeLogger.Web.Services
{
	public interface IUserService
	{
		Profile Authenticate(string username, string password);
		IEnumerable<Profile> GetAll();
	}
	public class UserService : IUserService
	{
		// users hardcoded for simplicity, store in a db with hashed passwords in production applications
		private List<Profile> _users = new List<Profile>
		{
			new Profile {  UserId = 0 , Name = "Sohail", Password ="pass",  DateOfBirth = DateTimeOffset.Now, EmpId = 0 }
		};


		private readonly AppSettings _appSettings;

		public UserService(IOptions<AppSettings> appSettings)
		{
			_appSettings = appSettings.Value;
		}

		public Profile Authenticate(string username, string password)
		{
			var user = _users.SingleOrDefault(x => x.Name == username && x.Password == password);

			// return null if user not found
			if (user == null)
				return null;

			// authentication successful so generate jwt token
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.UserId.ToString())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			user.Token = tokenHandler.WriteToken(token);

			return user.WithoutPassword();
		}

		public IEnumerable<Profile> GetAll()
		{
			return _users.WithoutPasswords();
		}
	}
}
