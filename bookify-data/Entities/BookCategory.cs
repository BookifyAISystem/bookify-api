using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class BookCategory
	{
		public int BookCategoryId { get; set; } // Đổi tên cho dễ hiểu
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }
		public int CategoryId { get; set; }
		public int BookId { get; set; }

		// Navigation properties
		public Category? Category { get; set; }
		public Book? Book { get; set; }
	}

}
