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
		public int? AccountId { get; set; } // Có thể null nếu không bắt buộc
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation properties
		public Account? Account { get; set; }

		public List<Bookshelf> Bookshelves { get; set; } = new List<Bookshelf>();
		public List<Order> Orders { get; set; } = new List<Order>();
		public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
		public Wishlist? Wishlist { get; set; }
		public List<Wishlist> Wishlists { get; set; } = new List<Wishlist>();

	}

}
