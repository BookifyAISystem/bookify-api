using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Promotion
	{
		public int PromotionId { get; set; }
		public string? Content { get; set; }
		public int Amount { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int Status { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }

		// Navigation property
		public List<Book> Books { get; set; } = new List<Book>();
	}

}
