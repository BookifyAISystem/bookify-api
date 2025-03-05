using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookify_data.Repository
{
    public class BookshelfDetailRepository : IBookshelfDetailRepository
    {
        private readonly BookifyDbContext _dbContext;

        public BookshelfDetailRepository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<BookshelfDetail>> GetAllBookshelfDetailsAsync()
        {
            return await _dbContext.BookshelfDetails
                .Where(bsd => bsd.Status == 1) // Chỉ lấy sách chưa bị xóa
                .ToListAsync();
        }

        public async Task<BookshelfDetail?> GetBookshelfDetailByIdAsync(int id)
        {
            return await _dbContext.BookshelfDetails
                .Where(bsd => bsd.BookshelfDetailId == id && bsd.Status == 1)
                .FirstOrDefaultAsync();
        }

        public async Task AddBookshelfDetailAsync(BookshelfDetail bookshelfDetail)
        {
            _dbContext.BookshelfDetails.Add(bookshelfDetail);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBookshelfDetailAsync(BookshelfDetail bookshelfDetail)
        {
            bookshelfDetail.LastEdited = DateTime.UtcNow; // Cập nhật thời gian chỉnh sửa
            _dbContext.BookshelfDetails.Update(bookshelfDetail);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBookshelfDetailAsync(int id)
        {
            var bookshelfDetail = await _dbContext.BookshelfDetails.FindAsync(id);
            if (bookshelfDetail != null)
            {
                bookshelfDetail.Status = 0; // Soft Delete
                bookshelfDetail.LastEdited = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task UpdateStatusAsync(int bookshelfDetailId, int status)
        {
            var bookshelfDetail = await _dbContext.BookshelfDetails.FindAsync(bookshelfDetailId);
            if (bookshelfDetail != null)
            {
                bookshelfDetail.Status = status;
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
