using bookify_data.DTOs.WishlistDetailDTO;

namespace bookify_data.DTOs.WishlistDTO
{
    public class GetWishlistDTO
    {
        public int WishlistId { get; set; }
        public int AccountId { get; set; }
        public string? WishlistName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastEdited { get; set; }
        public int Status { get; set; }
        public List<GetWishlistDetailDTO> WishlistDetails { get; set; } = new List<GetWishlistDetailDTO>();
    }
}
