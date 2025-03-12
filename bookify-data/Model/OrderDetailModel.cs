using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
    public class GetOrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }

    }



    public class AddOrderDetailDTO
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
    public class UpdateOrderDetailDTO  // may be no need
    {
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Status { get; set; }

    }
}
