using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class OrderDetail
	{
		public int OrderdetailId { get; set; }
		public int Quantity { get; set; }
		public int OrderId { get; set; }
		public int BookId { get; set; }

		// Navigation properties
		public Order? Order { get; set; }
		public Book? Book { get; set; }
	}
}
