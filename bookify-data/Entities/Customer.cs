using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Customer
	{
		public int CustomerId { get; set; }
		public int AccountId { get; set; } 

		// Navigation properties
		public Account? Account { get; set; }
		public List<Order> Orders { get; set; } = new List<Order>();
		public List<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
		public List<BookShelf> BookShelves { get; set; } = new List<BookShelf>();
		public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
	}
}
