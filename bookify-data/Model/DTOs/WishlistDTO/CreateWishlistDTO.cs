namespace bookify_data.DTOs.WishlistDTO
{
    public class CreateWishlistDTO
    {
        public int AccountId { get; set; }
        public string? WishlistName { get; set; }
        public int Status { get; set; } = 1;  // Mặc định là 1 (Active)
    }
}
