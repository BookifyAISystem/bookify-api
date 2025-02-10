using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Helper
{
	public static class Consts
	{

		public class String
		{
			public const string KEY_CRYTO = "AIDDFĐFFĐFDFSDGDT";
			public static string RCPconnectionString = "";
			public static string POSconnectionString = "";
			public static string NotifyConnectionString = "";
		}
		public class QR
		{
			public static string VT = "VT";
			public static string PN = "PN";
			public static string PX = "PX";
		}
		public class Num
		{
			/// <summary>
			/// Limit for rows database
			/// </summary>
			public const int Limit = 50;
		}

		public class Path
		{
			public static string COMPLAIN = "upload/complains/{0}";//customerid
			public static string AVATAR = "upload/Avatar";//customerid
			public static string CUS_INFO = "upload/info";//customerid
			public static string BANNER = "upload/banner";//customerid
			public static string BANNER_FILES = "upload/banner/files";//customerid

			public static string NOTIFICATION = "upload/notifications";//customerid
			public static string PROMOTION = "upload/promotions";//customerid

			public static string IMG_SHIP = "upload/ship";
			public static string ICON = "upload/icons";

			public static string MESSAGE = "upload/messages/{0}";
		}

		public class Data
		{
			public const string FileNameEmpty = "*empty.png";

			public static readonly string[] VALID_EXTENSION_FILE_IMAGES = new string[] { ".png", ".jpeg", ".jpg", ".bmp", ".gif", ".webp" };
		}

		public class UserClaim
		{
			public const string Id = "id";
		}
	}
}
