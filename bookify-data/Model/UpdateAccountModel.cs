using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
	public class UpdateAccountModel
	{
		public string? UserName { get; set; }
		public string? DisplayName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
	}
}
