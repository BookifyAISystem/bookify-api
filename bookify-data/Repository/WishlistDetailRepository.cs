using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bookify_data.Repositories
{
    public class WishlistDetailRepository : IWishlistDetailRepository
    {
        private readonly BookifyDbContext _dbContext;

        public WishlistDetailRepository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<WishlistDetail>> GetAllWishlistDetailsAsync()
        {
            return await _dbContext.WishlistDetails.Include(wd => wd.Book).ToListAsync();
        }

        public async Task<WishlistDetail?> GetWishlistDetailByIdAsync(int wishlistDetailId)
        {
            return await _dbContext.WishlistDetails.Include(wd => wd.Book)
                         .FirstOrDefaultAsync(wd => wd.WishlistDetailId == wishlistDetailId);
        }

        public async Task AddWishlistDetailAsync(WishlistDetail wishlistDetail)
        {
            await _dbContext.WishlistDetails.AddAsync(wishlistDetail);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateWishlistDetailAsync(WishlistDetail wishlistDetail)
        {
            _dbContext.WishlistDetails.Update(wishlistDetail);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteWishlistDetailAsync(int wishlistDetailId)
        {
            var wishlistDetail = await GetWishlistDetailByIdAsync(wishlistDetailId);
            if (wishlistDetail != null)
            {
                _dbContext.WishlistDetails.Remove(wishlistDetail);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
