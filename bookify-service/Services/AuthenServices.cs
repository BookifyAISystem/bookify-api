using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static bookify_data.Helper.CacheKey;

namespace bookify_service.Services
{
	public class AuthenServices : IAuthenServices
	{
		private readonly IAuthenRepository authenRepository;
/*		private readonly IEmailSender emailSenderRepository;
*/
		public AuthenServices(IAuthenRepository authenRepository
			/*, IEmailSender emailSenderRepository*/
			)
		{
			this.authenRepository = authenRepository;
			/*this.emailSenderRepository = emailSenderRepository;*/
		}
		public Task<string> Login(LoginModel model)
		{
			return authenRepository.Login(model);
		}

		public Task<string> Register(RegisterLoginModel registerDTO)
		{
			return authenRepository.Register(registerDTO);
		}
		/*public async Task<string> ConfirmEmailAsync(string? username)
		{
			return await emailSenderRepository.ConfirmEmailAsync(username);
		}*/

		public string GenerateJwtToken(Account user)
		{
			return authenRepository.GenerateJwtToken(user);
		}
		public async Task<bool> Logout(HttpContext httpContext)
		{
			httpContext.Response.Cookies.Delete("authToken");
			return await Task.FromResult(true);
		}

		public bool IsTokenBlacklisted(string token)
		{
			return false;
		}
	}
}
