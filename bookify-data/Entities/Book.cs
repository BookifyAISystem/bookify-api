using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Book
	{
		public int BookId { get; set; }
		public string? BookName { get; set; }
		public string? BookImage { get; set; }
		public string? BookType { get; set; }
		public int? Price { get; set; }
		public int? PriceEbook { get; set; }
		public string? Description { get; set; }
		public string? BookContent { get; set; }

		
		public int PulishYear { get; set; }

		public DateTime CreateDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		public int AuthorId { get; set; }
		public int CategoryId { get; set; }
		public int CollectionId { get; set; }
		public int PromotionId { get; set; }

		public Promotion? Promotion { get; set; }
		public Author? Author { get; set; }
		public Collection? Collection { get; set; }
		public Category? Category { get; set; }

		public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
		public List<WishlistDetail> WishlistDetails { get; set; } = new List<WishlistDetail>();
		public List<BookShelfDetail> BookShelfDetails { get; set; } = new List<BookShelfDetail>();
		public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
	}
}
