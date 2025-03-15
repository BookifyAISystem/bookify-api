using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
    public class PaymentModel
    {
        public int PaymentId { get; set; }
        public int Method { get; set; }
        public DateTime PaymentDate { get; set; }
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }
    }
}
