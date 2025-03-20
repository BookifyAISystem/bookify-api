using bookify_data.DTOs.WishlistDTO;

namespace bookify_service.Interfaces
{
    public interface IWishlistService
    {
        Task<IEnumerable<GetWishlistDTO>> GetAllWishlistsAsync();
        Task<GetWishlistDTO?> GetWishlistByIdAsync(int wishlistId);
        Task AddWishlistAsync(CreateWishlistDTO wishlistDto);
        Task UpdateWishlistAsync(UpdateWishlistDTO wishlistDto);
        Task DeleteWishlistAsync(int wishlistId);
    }
}
