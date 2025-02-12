using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
	public class AuthenticateResponse
	{
		public decimal Id { get; set; }
		public string Phone { get; set; }
		public string FullName { get; set; }
		public string Token { get; set; }
		public byte Status { get; set; }
		public bool isAdmin { get; set; }
		public string claims { get; set; }
		public string RefreshTokens { get; set; }
		public int IsVerified { get; set; }
		public string Email { get; set; }
		//public UserType UserType { get; set; }
		public AuthenticateResponse()
		{

		}

		//public AuthenticateResponse(User user)
		//{
		//    Id = user.Id;
		//    customerID = user.CustomerID;
		//    Phone = user.ContactPhone;
		//    FullName = user.FullName;
		//    Token = "";
		//    UserType = user.UserType;

		//    isAdmin = user.RoleID == "1" ? true : false;
		//}

		public AuthenticateResponse(Account user, string token, string claims)
		{
			Id = user.AccountId;
			Email = user.Email;
			FullName = user.DisplayName;
			Token = token;
			Email = user.Email;
			this.claims = claims;
		}
		public AuthenticateResponse(Account user, string token, string refesh_token, string claims)
		{
			Id = user.AccountId;
			Email = user.Email;
			FullName = user.DisplayName ;
			Token = token;
			this.claims = claims;
			RefreshTokens = refesh_token;
		}
	}
}
