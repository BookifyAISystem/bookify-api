using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model.OrderModel
{
    public class UpdateOrderDTO
    {
        [Column("order_id")]
        public int OrderId { get; set; }


        [Column("total")]
        public int Total { get; set; } 

        [Column("create_date")]
        public DateTime CreateDate { get; set; }

        [Column("cancel_reason")]
        public string? CancelReason { get; set; }

    }
}
