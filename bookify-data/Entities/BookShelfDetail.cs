using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class BookShelfDetail
	{
		public int BookshelfdetailId { get; set; }
		public int BookshelfId { get; set; }
		public int BookId { get; set; }

		// Navigation properties
		public BookShelf? BookShelf { get; set; }
		public Book? Book { get; set; }
	}
}
