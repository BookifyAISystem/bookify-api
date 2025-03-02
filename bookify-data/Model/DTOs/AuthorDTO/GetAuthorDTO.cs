namespace bookify_data.DTOs.AuthorDTO
{
    public class GetAuthorDTO
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }
    }
}
