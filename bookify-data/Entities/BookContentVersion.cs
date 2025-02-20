using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class BookContentVersion
	{
		public int BookContentVersionId { get; set; }
		public int BookId { get; set; }
		public string? AiSummary { get; set; }
		public int Version { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation property
		public Book? Book { get; set; }
	}

}
