using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bookify_data.Repositories
{
    public class BookContentVersionRepository : IBookContentVersionRepository
    {
        private readonly BookifyDbContext _context;

        public BookContentVersionRepository(BookifyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BookContentVersion entity)
        {
            await _context.BookContentVersions.AddAsync(entity);
        }

        public async Task<BookContentVersion?> GetByIdAsync(int id)
        {
            return await _context.BookContentVersions.FindAsync(id);
        }

        public async Task<List<BookContentVersion>> GetAllVersionsByBookIdAsync(int bookId)
        {
            return await _context.BookContentVersions
                .Where(b => b.BookId == bookId)
                .OrderByDescending(b => b.Version)
                .ToListAsync();
        }

        public async Task UpdateAsync(BookContentVersion entity)
        {
            _context.BookContentVersions.Update(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.BookContentVersions.Remove(entity);
                await SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // 🟢 Thêm hàm `GetLatestVersionByBookIdAsync` để lấy phiên bản mới nhất của Book
        public async Task<BookContentVersion?> GetLatestVersionByBookIdAsync(int bookId)
        {
            return await _context.BookContentVersions
                .Where(b => b.BookId == bookId)
                .OrderByDescending(b => b.Version)
                .FirstOrDefaultAsync();
        }
    }
}
