using bookify_data.DTOs;
using bookify_data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int bookId);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        void UpdateBook(Book book);
        Task DeleteBookAsync(int bookId);
        Task SaveChangesAsync();
        Task<(IEnumerable<GetBookDTO>, int)> SearchBooksAsync(string query, int pageNumber, int pageSize);
        Task<IEnumerable<GetBookDTO>> SuggestBooksAsync(string query, int limit);
        IQueryable<Book> QueryBooks();
        Task UpdateStatusAsync(int bookId, int status);
        Task<IEnumerable<Book>> GetLatestBooksAsync(int count);
        Task<IEnumerable<Book>> GetBestSellingBooksAsync(int count);
        Task UpdateBookQuantityAsync(int bookId, int quantity);


    }
}
