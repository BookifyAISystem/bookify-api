using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class BookShelf
	{
		public int BookshelfId { get; set; }
		public int CustomerId { get; set; }

		// Navigation properties
		public Customer? Customer { get; set; }
		public List<BookShelfDetail> BookShelfDetails { get; set; } = new List<BookShelfDetail>();
	}
}
