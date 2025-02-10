using bookify_api.DTOs.BookDTO;
using bookify_data.Entities;

namespace bookify_api.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<GetBooksDTO>> GetAllBooksAsync();
        Task<GetBookDTO?> GetBookByIdAsync(int bookId);
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int bookId);
        Task<Book?> GetBookEntityByIdAsync(int bookId);  // Lấy entity Book

    }
}