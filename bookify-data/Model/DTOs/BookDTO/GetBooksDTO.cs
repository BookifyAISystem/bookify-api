using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_api.DTOs.BookDTO
{
    public class GetBooksDTO
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? BookImage { get; set; }
        public string? BookType { get; set; }
        public int? Price { get; set; }
        public int PulishYear { get; set; }
    }
}
