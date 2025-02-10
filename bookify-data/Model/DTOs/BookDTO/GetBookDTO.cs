namespace bookify_api.DTOs.BookDTO
{
    public class GetBookDTO
    {
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? BookImage { get; set; }
        public string? BookType { get; set; }
        public int? Price { get; set; }
        public string? Description { get; set; }
        public int PulishYear { get; set; }

        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int CollectionId { get; set; }
        public int PromotionId { get; set; }

        public string? AuthorName { get; set; }
        public string? CollectionName { get; set; }
        public string? CategoryName { get; set; }
    }
}
