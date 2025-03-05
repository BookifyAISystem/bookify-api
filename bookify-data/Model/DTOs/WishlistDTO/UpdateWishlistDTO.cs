namespace bookify_data.DTOs.WishlistDTO
{
    public class UpdateWishlistDTO
    {
        public int WishlistId { get; set; }
        public string? WishlistName { get; set; }
        public int Status { get; set; }
    }
}
