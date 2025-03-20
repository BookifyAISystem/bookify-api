using bookify_data.Entities;
using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static bookify_data.Helper.CacheKey;

namespace bookify_data.Interfaces
{
	public interface IAuthenRepository
	{

		Task<string> Login(LoginModel model);
		Task<string> Register(RegisterLoginModel registerDTO);
		string GenerateJwtToken(Account user);
		//Task<String> GetUserNameFromToken(HttpClient client);
		Task<bool> Logout(string userId);
		Task<string> LoginGoogle(GoogleLoginModel model);
		Task<string> ChangePassword(ChangePasswordModel model);


    }
}
