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
        Task DeleteBookAsync(int bookId);
        Task SaveChangesAsync();
        IQueryable<Book> QueryBooks();

    }
}
