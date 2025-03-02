using bookify_data.DTOs.WishlistDTO;
using bookify_data.Entities;

public interface IWishlistRepository
{
    Task<IEnumerable<GetWishlistDTO>> GetAllWishlistsAsync();
    Task<GetWishlistDTO?> GetWishlistByIdAsync(int wishlistId);
    Task<Wishlist?> GetWishlistEntityByIdAsync(int wishlistId);  
    Task AddWishlistAsync(Wishlist wishlist);
    Task UpdateWishlistAsync(Wishlist wishlist);
    Task DeleteWishlistAsync(int wishlistId);
}
