using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class BookAuthor
	{
		public int BookAuthorId { get; set; }
		public int BookId { get; set; }
		public int AuthorId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation properties
		public Book? Book { get; set; }
		public Author? Author { get; set; }
	}

}
