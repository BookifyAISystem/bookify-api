using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
	public class RegisterRequest
	{
		public long RCPCustomer { get; set; } = 0;
		public string ContactPhone { get; set; } = "";

		[Required]
		[MaxLength(255)]
		[MinLength(1)]
		public string FirstName { get; set; } = "";

		[MaxLength(255)]
		public string LastName { get; set; } = "";

		[MaxLength(5)]
		public string Zip { get; set; } = "";

		public string Password { get; set; } = "";

		public string PasswordRepeat { get; set; } = "";

		public string PortalCode { get; set; } = "+1";
		public string Email { get; set; } = "";
		public bool isMail { get; set; } = false;
		public DateTime? BirthDay { get; set; } = null;
		public string DeviceID { get; set; }
	}
}
