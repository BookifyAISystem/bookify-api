using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class WishlistDetail
	{
		public int WishlistdetailId { get; set; }
		public int BookId { get; set; }
		public int WishlistId { get; set; }

		// Navigation properties
		public Book? Book { get; set; }
		public Wishlist? Wishlist { get; set; }
	}
}
