using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
    public class NoteViewModel
    {
        public int NoteId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }
        public int AccountId { get; set; }

        // Bạn có thể muốn hiển thị thêm thông tin về Account
        // nhưng KHÔNG chứa cả entity Account để tránh vòng lặp
        public string? AccountDisplayName { get; set; }
    }
}
