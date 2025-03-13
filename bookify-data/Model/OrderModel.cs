using bookify_data.Entities;
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
        public int AccountId { get; set; }
        public int? VoucherId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; } // 1 - active , 0 - canceled , 2 - completed
        public List<GetOrderDetailDTO> OrderDetails { get; set; }

    }

    

    public class AddOrderDTO
    {
        public int AccountId { get; set; }
        public int? VoucherId { get; set; }
        public List<AddOrderDetailDTO> OrderDetails { get; set; }
    }
    public class UpdateOrderDTO
    {

        public int Status { get; set; }
        public string? CancelReason { get; set; }

    }

   
    
    
}
