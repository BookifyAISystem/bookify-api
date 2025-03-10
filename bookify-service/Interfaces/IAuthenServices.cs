using bookify_data.Entities;
using bookify_data.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static bookify_data.Helper.CacheKey;

namespace bookify_service.Interfaces
{
	public interface IAuthenServices
	{
		Task<string> Login(LoginModel model);
		Task<string> LoginGoogle(GoogleLoginModel model);
		Task<string> Register(RegisterLoginModel registerDTO);
/*		Task<string> ConfirmEmailAsync(string? username);
*/		string GenerateJwtToken(Account user);
		//Task<string> GetUsername
		Task<bool> Logout(HttpContext httpContext);
		bool IsTokenBlacklisted(string token);
	}
}
