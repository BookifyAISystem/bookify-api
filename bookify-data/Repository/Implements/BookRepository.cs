using bookify_api.DTOs.BookDTO;
using bookify_api.Repositories;
using bookify_data.Data;
using bookify_data.Entities;
using Microsoft.EntityFrameworkCore;

namespace bookify_api.Repositories.Implementations
{
    public class BookRepository : IBookRepository
    {
        private readonly BookifyDbContext _dbContext;

        public BookRepository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GetBooksDTO>> GetAllBooksAsync()
        {
            return await _dbContext.Books
                .Select(book => new GetBooksDTO
                {
                    BookId = book.BookId,
                    BookName = book.BookName,
                    BookImage = book.BookImage,
                    BookType = book.BookType,
                    Price = book.Price,
                    PulishYear = book.PulishYear
                })
                .ToListAsync();
        }

        public async Task<GetBookDTO?> GetBookByIdAsync(int bookId)
        {
            return await _dbContext.Books
                .Where(book => book.BookId == bookId)
                .Select(book => new GetBookDTO
                {
                    BookId = book.BookId,
                    BookName = book.BookName,
                    BookImage = book.BookImage,
                    BookType = book.BookType,
                    Price = book.Price,
                    Description = book.Description,
                    PulishYear = book.PulishYear,
                    AuthorName = book.Author.AuthorName,
                    CollectionName = book.Collection.CollectionName,
                    CategoryName = book.Category.CategoryName
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddBookAsync(Book book)
        {
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Book book)
        {
            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBookAsync(int bookId)
        {
            var book = await _dbContext.Books.FindAsync(bookId);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<Book?> GetBookEntityByIdAsync(int bookId)
        {
            return await _dbContext.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
        }
    }
}