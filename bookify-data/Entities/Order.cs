using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Order
	{
		public int OrderId { get; set; }
		public int Total { get; set; }
		public string? CancelReason { get; set; }
		public int CustomerId { get; set; }
		public int? VoucherId { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime LastEdited { get; set; }
		public int Status { get; set; }

		// Navigation properties
		public Customer? Customer { get; set; }
		public Voucher? Voucher { get; set; }
		public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
		public List<Payment> Payments { get; set; } = new List<Payment>();
	}

}
