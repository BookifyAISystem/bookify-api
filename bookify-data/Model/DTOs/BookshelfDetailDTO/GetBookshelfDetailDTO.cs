using System;

namespace bookify_data.DTOs.BookshelfDetailDTO
{
    public class GetBookshelfDetailDTO
    {
        public int BookshelfDetailId { get; set; }
        public int BookshelfId { get; set; }
        public int BookId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }
    }
}
