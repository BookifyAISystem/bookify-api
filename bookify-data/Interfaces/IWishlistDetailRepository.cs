using bookify_data.Entities;

namespace bookify_data.Interfaces
{
    public interface IWishlistDetailRepository
    {
        Task<IEnumerable<WishlistDetail>> GetAllWishlistDetailsAsync();
        Task<WishlistDetail?> GetWishlistDetailByIdAsync(int wishlistDetailId);
        Task AddWishlistDetailAsync(WishlistDetail wishlistDetail);
        Task UpdateWishlistDetailAsync(WishlistDetail wishlistDetail);
        Task DeleteWishlistDetailAsync(int wishlistDetailId);
    }
}
