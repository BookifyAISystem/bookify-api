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
		public int Price { get; set; }
		public int PriceEbook { get; set; }
		public string? Description { get; set; }
		public string? BookContent { get; set; }
		public int PublishYear { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; } = 1;
		public int? AuthorId { get; set; }
		public int? ParentBookId { get; set; }
		public int? CategoryId { get; set; }
		public int? PromotionId { get; set; }

		// Navigation properties
		public Book? ParentBook { get; set; }
		public Promotion? Promotion { get; set; }
		public Author? Author { get; set; }
		public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
		public List<BookshelfDetail> BookshelfDetails { get; set; } = new List<BookshelfDetail>();
		public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
		public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
		public List<WishlistDetail> WishlistDetails { get; set; } = new List<WishlistDetail>();
		public List<BookContentVersion> BookContentVersions { get; set; } = new List<BookContentVersion>();
		public List<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
	}

}
