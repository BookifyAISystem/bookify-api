using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Helper
{
	public static class CacheKey
	{
		/*public static string FieldStartWith = "Field";
		public static string PriceStartWith = "Price";
		public static string ProvinceID = "ProvinceID";
		public static string ServerID = "ServerID";

		public static string Menu = "Menu";
		public static string Cancels = "Cancels";

		public static string STORAGE_TIME = "StorageTime";*/


		public static string Logo = "Logo";
		public class User
		{
			public static string UserLogin = "UserLogin.{0}.";
			public static string CustomerID = "UserCustomerID.{0}.";
			public static string UserRoles = "UserRoles.{0}.";
			public static string UserStartWith = "User";
			public static string UserToken = "UserToken.{0}.";
			public static string UserRefreshToken = "UserRefreshToken.{0}";
			public static string UserCharge = "UserCharge.{0}";
			public static string Register = "Register.{0}.";
			public static string ResetPassword = "ResetPassword.{0}.";
		}

		public class Zone
		{
			public static string ZoneStartWith = "Zone";
			public static string ZoneIds = "Zone.{0}";
		}

	}
}
