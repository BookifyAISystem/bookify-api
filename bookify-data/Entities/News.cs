using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class News
	{
		public int NewsId { get; set; }
		public string? Title { get; set; }
		public string? Content { get; set; }
		public string? Summary { get; set; }
		public string? ImageUrl { get; set; }
		public int Views { get; set; }
		public DateTime PublishAt { get; set; }
		public int AccountId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation property
		public Account? Account { get; set; }
	}

}
