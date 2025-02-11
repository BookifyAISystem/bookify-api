﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Entities
{
	public class Order
	{
        [Column("order_id")]
        public int OrderId { get; set; }

        [Column("total")]
        public int Total { get; set; } // DB ghi là "toltal", có thể sửa thành "total" nếu muốn

        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        [Column("status")]
        public int Status { get; set; }
        // Cột "cancel reason" có dấu cách, nên có thể sửa lại trong DB cho chuẩn.

        [Column("cancel_reason")]
        public string? CancelReason { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("voucher_id")]
        public int VoucherId { get; set; }

		// Navigation properties
		public Customer? Customer { get; set; }
		public Voucher? Voucher { get; set; }
		public List<Payment> Payments { get; set; } = new List<Payment>();
		public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
	}
}
