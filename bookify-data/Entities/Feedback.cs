using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Feedback
	{
		public int FeedbackId { get; set; }
		public int Star { get; set; }
		public string? FeedbackContent { get; set; }
		public int CustomerId { get; set; }
		public int BookId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation properties
		public Account? Account { get; set; }
		public Book? Book { get; set; }
	}

}
