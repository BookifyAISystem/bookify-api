using bookify_data.DTOs.WishlistDTO;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_service.Interfaces;

namespace bookify_service.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<IEnumerable<GetWishlistDTO>> GetAllWishlistsAsync()
        {
            return await _wishlistRepository.GetAllWishlistsAsync();
        }

        public async Task<GetWishlistDTO?> GetWishlistByIdAsync(int wishlistId)
        {
            return await _wishlistRepository.GetWishlistByIdAsync(wishlistId);
        }

        public async Task AddWishlistAsync(CreateWishlistDTO wishlistDto)
        {
            var wishlist = new Wishlist
            {
                AccountId = wishlistDto.AccountId,
                WishlistName = wishlistDto.WishlistName,
                CreatedDate = DateTime.UtcNow,
                LastEdited = DateTime.UtcNow,
                Status = wishlistDto.Status
            };

            await _wishlistRepository.AddWishlistAsync(wishlist);
        }

        public async Task UpdateWishlistAsync(UpdateWishlistDTO wishlistDto)
        {
            // Lấy wishlist từ database dưới dạng entity
            var wishlistEntity = await _wishlistRepository.GetWishlistEntityByIdAsync(wishlistDto.WishlistId);
            if (wishlistEntity == null) throw new KeyNotFoundException("Wishlist không tồn tại!");

            // Cập nhật thông tin
            wishlistEntity.WishlistName = wishlistDto.WishlistName;
            wishlistEntity.LastEdited = DateTime.UtcNow;
            wishlistEntity.Status = wishlistDto.Status;

            // Gọi repository để update
            await _wishlistRepository.UpdateWishlistAsync(wishlistEntity);
        }

        public async Task DeleteWishlistAsync(int wishlistId)
        {
            await _wishlistRepository.DeleteWishlistAsync(wishlistId);
        }
    }
}
