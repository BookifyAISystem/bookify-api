namespace bookify_data.DTOs
{
    public class GetBookDTO
    {
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string? BookImage { get; set; } // Trả về đường dẫn ảnh từ AWS S3
        public string? BookType { get; set; }
        public int Price { get; set; }
        public int PriceEbook { get; set; }
        public string? Description { get; set; }
        public string? BookContent { get; set; }
        public int PublishYear { get; set; }
        public int CategoryId { get; set; }
        public int PromotionId { get; set; }
        public int? ParentBookId { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
    }
}
