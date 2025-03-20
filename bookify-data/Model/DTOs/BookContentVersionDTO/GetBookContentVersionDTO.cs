namespace bookify_data.DTOs.BookContentVersionDTO
{
    public class GetBookContentVersionDTO
    {
        public int BookContentVersionId { get; set; }
        public int BookId { get; set; }
        public List<string> Summaries { get; set; } = new List<string>();
        public int Version { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }
    }
}
