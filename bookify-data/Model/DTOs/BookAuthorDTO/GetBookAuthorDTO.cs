namespace bookify_data.DTOs.BookAuthorDTO
{
    public class GetBookAuthorDTO
    {
        public int BookAuthorId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
}
