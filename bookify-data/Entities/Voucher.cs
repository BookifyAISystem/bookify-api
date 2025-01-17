using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Voucher
	{
		public int VoucherId { get; set; }
		public string? VoucherCode { get; set; }
		public int Discount { get; set; }
		public int MinAmount { get; set; }
		public int MaxDiscount { get; set; }
		public int Quantity { get; set; }
		public int Status { get; set; }

		// Navigation property
		public List<Order> Orders { get; set; } = new List<Order>();
	}
}
