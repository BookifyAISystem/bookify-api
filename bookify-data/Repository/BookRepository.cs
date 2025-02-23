using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookify_data.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookifyDbContext _context;

        public BookRepository(BookifyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .Where(b => b.Status == 1) // Chỉ lấy sách chưa bị xóa
                .ToListAsync();
        }


        public async Task<Book?> GetBookByIdAsync(int bookId)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
        }

        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book với ID {bookId} không tồn tại.");
            }

            book.Status = 0; // Đánh dấu sách là đã bị xóa thay vì xóa khỏi DB
            book.LastEdited = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
