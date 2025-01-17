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
		public int Toltal { get; set; } // DB ghi là "toltal", có thể sửa thành "total" nếu muốn
		public DateTime CreateDate { get; set; }
		public bool Status { get; set; }
		// Cột "cancel reason" có dấu cách, nên có thể sửa lại trong DB cho chuẩn.
		public string? CancelReason { get; set; }

		public int CustomerId { get; set; }
		public int VoucherId { get; set; }

		// Navigation properties
		public Customer? Customer { get; set; }
		public Voucher? Voucher { get; set; }
		public List<Payment> Payments { get; set; } = new List<Payment>();
		public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
	}
}
