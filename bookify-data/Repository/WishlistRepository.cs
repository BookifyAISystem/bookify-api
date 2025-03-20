using bookify_data.Data;
using bookify_data.DTOs.WishlistDTO;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bookify_data.Repository
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly BookifyDbContext _dbContext;

        public WishlistRepository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GetWishlistDTO>> GetAllWishlistsAsync()
        {
            return await _dbContext.Wishlists
                .Select(w => new GetWishlistDTO
                {
                    WishlistId = w.WishlistId,
                    AccountId = w.AccountId,
                    WishlistName = w.WishlistName,
                    CreatedDate = w.CreatedDate,
                    LastEdited = w.LastEdited,
                    Status = w.Status
                })
                .ToListAsync();
        }

        public async Task<GetWishlistDTO?> GetWishlistByIdAsync(int wishlistId)
        {
            return await _dbContext.Wishlists
                .Where(w => w.WishlistId == wishlistId)
                .Select(w => new GetWishlistDTO
                {
                    WishlistId = w.WishlistId,
                    AccountId = w.AccountId,
                    WishlistName = w.WishlistName,
                    CreatedDate = w.CreatedDate,
                    LastEdited = w.LastEdited,
                    Status = w.Status
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddWishlistAsync(Wishlist wishlist)
        {
            await _dbContext.Wishlists.AddAsync(wishlist);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateWishlistAsync(Wishlist wishlist)
        {
            _dbContext.Wishlists.Update(wishlist);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteWishlistAsync(int wishlistId)
        {
            var wishlist = await _dbContext.Wishlists.FindAsync(wishlistId);
            if (wishlist != null)
            {
                _dbContext.Wishlists.Remove(wishlist);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<Wishlist?> GetWishlistEntityByIdAsync(int wishlistId)
        {
            return await _dbContext.Wishlists.FirstOrDefaultAsync(w => w.WishlistId == wishlistId);
        }

    }
}
