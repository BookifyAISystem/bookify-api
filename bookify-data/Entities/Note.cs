using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Note
	{
		public int NoteId { get; set; }
		public string? Content { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }
		public int AccountId { get; set; }

		// Navigation property
		public Account? Account { get; set; }
	}

}
