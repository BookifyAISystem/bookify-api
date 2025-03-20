using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Bookshelf
	{
		public int BookshelfId { get; set; }
		public int AccountId { get; set; }
		public string? BookShelfName { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation properties
		public Account? Account { get; set; }
		public List<BookshelfDetail> BookshelfDetails { get; set; } = new List<BookshelfDetail>();
	}

}
