using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
    public class GetOrderDTO
    {
        public int OrderId { get; set; }
        public int Total { get; set; }
        public string? CancelReason { get; set; }
        public int CustomerId { get; set; }
        public int VoucherId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }

    }

    

    public class AddOrderDTO
    {
        public int Total { get; set; }
        public int CustomerId { get; set; }
        public int VoucherId { get; set; }
    }
    public class UpdateOrderDTO
    {

        public int Total { get; set; }
        public int CustomerId { get; set; }
        public int VoucherId { get; set; }

    }
    
    
}
