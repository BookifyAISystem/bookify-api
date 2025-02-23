using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
    public class UpdateOrderDTO
    {
        public int OrderId { get; set; }

        public int Total { get; set; }

        public DateTime CreateDate { get; set; }

        public string? CancelReason { get; set; }

    }
    public class GetOrdersDTO
    {
        public int OrderId { get; set; }

        public int Total { get; set; } 

        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
       
        public string? CancelReason { get; set; }

    }
    public class GetOrderDTO
    {
        public int OrderId { get; set; }

        public int Total { get; set; } // DB ghi là "toltal", có thể sửa thành "total" nếu muốn

        public DateTime CreateDate { get; set; }

        public int Status { get; set; }
        // Cột "cancel reason" có dấu cách, nên có thể sửa lại trong DB cho chuẩn.

        public string? CancelReason { get; set; }


    }
    public class AddOrderDTO
    {
        public int OrderId { get; set; }

        public int Total { get; set; } // DB ghi là "toltal", có thể sửa thành "total" nếu muốn

        public DateTime CreateDate { get; set; }

        public string? CancelReason { get; set; }

        public int CustomerId { get; set; }

        public int VoucherId { get; set; }
    }
}
