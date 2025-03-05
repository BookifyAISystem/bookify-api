using bookify_data.DTOs.WishlistDetailDTO;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_service.Interfaces;

namespace bookify_service.Services
{
    public class WishlistDetailService : IWishlistDetailService
    {
        private readonly IWishlistDetailRepository _wishlistDetailRepository;

        public WishlistDetailService(IWishlistDetailRepository wishlistDetailRepository)
        {
            _wishlistDetailRepository = wishlistDetailRepository;
        }

        public async Task<IEnumerable<GetWishlistDetailDTO>> GetAllWishlistDetailsAsync()
        {
            var wishlistDetails = await _wishlistDetailRepository.GetAllWishlistDetailsAsync();
            return wishlistDetails.Select(wd => new GetWishlistDetailDTO
            {
                WishlistDetailId = wd.WishlistDetailId,
                WishlistId = wd.WishlistId,
                BookId = wd.BookId,
                BookName = wd.Book?.BookName,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                LastEdited = DateTime.UtcNow.AddHours(7),
                Status = wd.Status
            });
        }

        public async Task<GetWishlistDetailDTO?> GetWishlistDetailByIdAsync(int wishlistDetailId)
        {
            var wd = await _wishlistDetailRepository.GetWishlistDetailByIdAsync(wishlistDetailId);
            return wd != null ? new GetWishlistDetailDTO
            {
                WishlistDetailId = wd.WishlistDetailId,
                WishlistId = wd.WishlistId,
                BookId = wd.BookId,
                BookName = wd.Book?.BookName,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                LastEdited = DateTime.UtcNow.AddHours(7),
                Status = wd.Status
            } : null;
        }

        public async Task AddWishlistDetailAsync(CreateWishlistDetailDTO wishlistDetailDto)
        {
            var wd = new WishlistDetail
            {
                WishlistId = wishlistDetailDto.WishlistId,
                BookId = wishlistDetailDto.BookId,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                LastEdited = DateTime.UtcNow.AddHours(7),
                Status = 1
            };
            await _wishlistDetailRepository.AddWishlistDetailAsync(wd);
        }

        public async Task UpdateWishlistDetailAsync(UpdateWishlistDetailDTO wishlistDetailDto)
        {
            var wd = await _wishlistDetailRepository.GetWishlistDetailByIdAsync(wishlistDetailDto.WishlistDetailId);
            if (wd != null)
            {
                wd.BookId = wishlistDetailDto.BookId;
                wd.LastEdited = DateTime.UtcNow.AddHours(7);
                wd.Status = wishlistDetailDto.Status;
                await _wishlistDetailRepository.UpdateWishlistDetailAsync(wd);
            }
        }

        public async Task DeleteWishlistDetailAsync(int wishlistDetailId)
        {
            await _wishlistDetailRepository.DeleteWishlistDetailAsync(wishlistDetailId);
        }
    }
}
