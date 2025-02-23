using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
    public class GetVoucherDTO
    {
        public int VoucherId { get; set; }
        public string? VoucherCode { get; set; }
        public int Discount { get; set; }
        public int MinAmount { get; set; }
        public int MaxDiscount { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }


    }



    public class AddVoucherDTO
    {
        public int VoucherId { get; set; }
        public string? VoucherCode { get; set; }
        public int Discount { get; set; }
        public int MinAmount { get; set; }
        public int MaxDiscount { get; set; }
        public int Quantity { get; set; }
    }
    public class UpdateVoucherDTO
    {
        public string? VoucherCode { get; set; }
        public int Discount { get; set; }
        public int MinAmount { get; set; }
        public int MaxDiscount { get; set; }
        public int Quantity { get; set; }

    }
}
