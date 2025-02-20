using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Author
	{
		public int AuthorId { get; set; }
		public string? AuthorName { get; set; }
		public string? Content { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation property
		public List<Book> Books { get; set; } = new List<Book>();
		public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
	}

}
