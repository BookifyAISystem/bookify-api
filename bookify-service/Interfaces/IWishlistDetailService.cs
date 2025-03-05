using bookify_data.DTOs.WishlistDetailDTO;

namespace bookify_service.Interfaces
{
    public interface IWishlistDetailService
    {
        Task<IEnumerable<GetWishlistDetailDTO>> GetAllWishlistDetailsAsync();
        Task<GetWishlistDetailDTO?> GetWishlistDetailByIdAsync(int wishlistDetailId);
        Task AddWishlistDetailAsync(CreateWishlistDetailDTO wishlistDetailDto);
        Task UpdateWishlistDetailAsync(UpdateWishlistDetailDTO wishlistDetailDto);
        Task DeleteWishlistDetailAsync(int wishlistDetailId);
    }
}
