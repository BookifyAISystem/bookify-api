using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
	public class AppSettings
	{
		private static 	IConfiguration _configuration;
		public static void Configure(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public static string Secret => _configuration["AppSettings:Secret"];
		public static string Url => _configuration["AppSettings:Url"];
		public static int ExpiredTime => Convert.ToInt32(_configuration["AppSettings:ExpiredTime"]);
	}
}
