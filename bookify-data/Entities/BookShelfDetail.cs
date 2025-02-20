using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class BookshelfDetail
	{
		public int BookshelfDetailId { get; set; }
		public int BookshelfId { get; set; }
		public int BookId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation properties
		public Bookshelf? Bookshelf { get; set; }
		public Book? Book { get; set; }
	}

}
