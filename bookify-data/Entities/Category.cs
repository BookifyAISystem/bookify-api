using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Category
	{
		public int CategoryId { get; set; }
		public string? CategoryName { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation properties
		public List<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
		public List<News> NewsList { get; set; } = new List<News>();
	}

}
