namespace bookify_data.DTOs.WishlistDetailDTO
{
    public class UpdateWishlistDetailDTO
    {
        public int WishlistDetailId { get; set; }
        public int WishlistId { get; set; }
        public int BookId { get; set; }
        public int Status { get; set; }  // Thêm trường này để cập nhật status nếu cần
    }
}
