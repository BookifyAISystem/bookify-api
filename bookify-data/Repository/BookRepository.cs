using bookify_data.Data;
using bookify_data.DTOs;
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
        // 🔍 Gợi ý sách khi nhập ký tự
        public async Task<IEnumerable<GetBookDTO>> SuggestBooksAsync(string query, int limit)
        {
            return await _context.Books
                .Where(b => b.Status == 1 && b.BookName.Contains(query)) // Chỉ lấy sách có status = 1
                .OrderBy(b => b.BookName)
                .Take(limit)
                .Select(b => new GetBookDTO
                {
                    BookId = b.BookId,
                    BookName = b.BookName,
                    BookImage = b.BookImage,
                    Price = b.Price,
                    PublishYear = b.PublishYear
                }).ToListAsync();
        }

        // 🔍 Phân trang khi tìm kiếm
        public async Task<(IEnumerable<GetBookDTO>, int)> SearchBooksAsync(string query, int pageNumber, int pageSize)
        {
            var booksQuery = _context.Books
                .Where(b => b.Status == 1 && b.BookName.Contains(query))
                .OrderBy(b => b.BookName);

            int totalRecords = await booksQuery.CountAsync(); // Tổng số sách
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize); // Tổng số trang

            var books = await booksQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new GetBookDTO
                {
                    BookId = b.BookId,
                    BookName = b.BookName,
                    BookImage = b.BookImage,
                    Price = b.Price,
                    PublishYear = b.PublishYear
                }).ToListAsync();

            return (books, totalPages);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public IQueryable<Book> QueryBooks()
        {
            return _context.Books.AsQueryable();
        }
        public async Task UpdateStatusAsync(int bookId, int status)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                book.Status = status;
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Book>> GetLatestBooksAsync(int count)
        {
            return await _context.Books
                .Where(b => b.Status == 1) 
                .OrderByDescending(b => b.CreatedDate) 
                .Take(count) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBestSellingBooksAsync(int count)
        {
            return await _context.Books
                .Where(b => b.Status == 1)
                .OrderByDescending(b => _context.OrderDetails
                    .Where(od => od.BookId == b.BookId)
                    .Sum(od => od.Quantity))
                .Take(count)
                .ToListAsync();
        }
        public async Task UpdateBookQuantityAsync(int bookId, int quantity)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book với ID {bookId} không tồn tại.");
            }

            book.Quantity += quantity; // ✅ Cộng thêm số lượng mới
            book.LastEdited = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

    }
}
