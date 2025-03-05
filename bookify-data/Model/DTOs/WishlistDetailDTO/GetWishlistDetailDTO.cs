namespace bookify_data.DTOs.WishlistDetailDTO
{
    public class GetWishlistDetailDTO
    {
        public int WishlistDetailId { get; set; }
        public int WishlistId { get; set; }
        public int BookId { get; set; }
        public string? BookName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }
    }
}
