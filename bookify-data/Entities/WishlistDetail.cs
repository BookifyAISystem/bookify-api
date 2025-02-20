using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class WishlistDetail
	{
		public int WishlistDetailId { get; set; }
		public int BookId { get; set; }
		public int WishlistId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation properties
		public Wishlist? Wishlist { get; set; }
		public Book? Book { get; set; }
	}

}
