using bookify_data.Data;
using bookify_data.DTOs.BookAuthorDTO;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookify_data.Repositories
{
    public class BookAuthorRepository : IBookAuthorRepository
    {
        private readonly BookifyDbContext _dbContext;

        public BookAuthorRepository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GetBookAuthorDTO>> GetAllBookAuthorsAsync()
        {
            return await _dbContext.BookAuthors
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .Select(ba => new GetBookAuthorDTO
                {
                    BookAuthorId = ba.BookAuthorId,
                    BookId = ba.BookId,
                    BookName = ba.Book!.BookName!,
                    AuthorId = ba.AuthorId,
                    AuthorName = ba.Author!.AuthorName!
                })
                .ToListAsync();
        }

        public async Task<GetBookAuthorDTO?> GetBookAuthorByIdAsync(int bookAuthorId)
        {
            return await _dbContext.BookAuthors
                .Where(ba => ba.BookAuthorId == bookAuthorId)
                .Include(ba => ba.Book)
                .Include(ba => ba.Author)
                .Select(ba => new GetBookAuthorDTO
                {
                    BookAuthorId = ba.BookAuthorId,
                    BookId = ba.BookId,
                    BookName = ba.Book!.BookName!,
                    AuthorId = ba.AuthorId,
                    AuthorName = ba.Author!.AuthorName!
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddBookAuthorAsync(CreateBookAuthorDTO bookAuthorDto)
        {
            var bookAuthor = new BookAuthor
            {
                BookId = bookAuthorDto.BookId,
                AuthorId = bookAuthorDto.AuthorId
            };

            await _dbContext.BookAuthors.AddAsync(bookAuthor);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBookAuthorAsync(UpdateBookAuthorDTO bookAuthorDto)
        {
            var bookAuthor = await _dbContext.BookAuthors.FindAsync(bookAuthorDto.BookAuthorId);
            if (bookAuthor != null)
            {
                bookAuthor.BookId = bookAuthorDto.BookId;
                bookAuthor.AuthorId = bookAuthorDto.AuthorId;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteBookAuthorAsync(int bookAuthorId)
        {
            var bookAuthor = await _dbContext.BookAuthors.FindAsync(bookAuthorId);
            if (bookAuthor != null)
            {
                _dbContext.BookAuthors.Remove(bookAuthor);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task UpdateStatusAsync(int bookAuthorId, int status)
        {
            var bookAuthor = await _dbContext.BookAuthors.FindAsync(bookAuthorId);
            if (bookAuthor != null)
            {
                bookAuthor.Status = status;
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
