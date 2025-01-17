using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Collection
	{
		public int CollectionId { get; set; }
		public string? CollectionName { get; set; }

		// Navigation property
		public List<Book> Books { get; set; } = new List<Book>();
	}
}
