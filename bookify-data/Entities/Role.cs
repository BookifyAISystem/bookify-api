using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Role
	{
		public int RoleId { get; set; }
		public string? RoleName { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation property
		public List<Account> Accounts { get; set; } = new List<Account>();
	}

}
