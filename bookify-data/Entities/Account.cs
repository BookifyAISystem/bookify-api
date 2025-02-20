using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Account
	{
		public int AccountId { get; set; }
		public string? Username { get; set; }
		public string? Password { get; set; }
		public string? DisplayName { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }
		public int RoleId { get; set; }

		// Navigation properties
		public Role? Role { get; set; }
		public List<News> NewsList { get; set; } = new List<News>();
		public List<Note> Notes { get; set; } = new List<Note>();
		public List<Customer> Customers { get; set; } = new List<Customer>();
	}

}
