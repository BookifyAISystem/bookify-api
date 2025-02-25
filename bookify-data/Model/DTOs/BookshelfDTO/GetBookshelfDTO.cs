using System;

namespace bookify_data.DTOs.BookshelfDTO
{
    public class GetBookshelfDTO
    {
        public int BookshelfId { get; set; }
        public int CustomerId { get; set; }
        public string BookShelfName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }
    }
}
