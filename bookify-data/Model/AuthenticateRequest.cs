using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
	public class AuthenticateRequest
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }
	}
	public class ChangePasswordModel
	{
		[Required]
        public string Email { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
	public class ResetPasswordModel
	{
		[Required]
		public string User { get; set; }
		[Required]
		public string Password { get; set; }
		[Required]
		public string PasswordRepeat { get; set; }
	}
}
