using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Model
{
    public class GetBookCategoryDTO
    {
        public int BookCategoryId { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public int BookId { get; set; }
    }

    public class AddBookCategoryDTO
    {
        public int CategoryId { get; set; }
        public int BookId { get; set; }
    }

    public class UpdateBookCategoryDTO
    {
        public string? CategoryName { get; set; }
        public int Status { get; set; }
    }
}
