using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Wishlist
	{
		public int WishlistId { get; set; }
		public int AccountId { get; set; }
        public string? WishlistName { get; set; }
        public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation properties
		public Account? Account { get; set; }
		public List<WishlistDetail> WishlistDetails { get; set; } = new List<WishlistDetail>();
	}

}
