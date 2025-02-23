using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_data.Repository
{
    public class BookshelfRepository : IBookshelfRepository
    {
        private readonly BookifyDbContext _dbContext;

        public BookshelfRepository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Bookshelf>> GetAllBookshelvesAsync()
        {
            return await _dbContext.Bookshelves.Where(bs => bs.Status == 1).ToListAsync();
        }

        public async Task<Bookshelf?> GetBookshelfByIdAsync(int bookshelfId)
        {
            return await _dbContext.Bookshelves.FindAsync(bookshelfId);
        }

        public async Task AddBookshelfAsync(Bookshelf bookshelf)
        {
            _dbContext.Bookshelves.Add(bookshelf);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBookshelfAsync(Bookshelf bookshelf)
        {
            _dbContext.Bookshelves.Update(bookshelf);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBookshelfAsync(int bookshelfId)
        {
            var bookshelf = await _dbContext.Bookshelves.FindAsync(bookshelfId);
            if (bookshelf != null)
            {
                bookshelf.Status = 0; // Soft delete
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
