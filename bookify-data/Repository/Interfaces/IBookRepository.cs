using bookify_api.DTOs.BookDTO;
using bookify_data.Entities;

public interface IBookRepository
{
    Task<IEnumerable<GetBooksDTO>> GetAllBooksAsync();
    Task<GetBookDTO?> GetBookByIdAsync(int bookId);
    Task<Book?> GetBookEntityByIdAsync(int bookId);
    Task<Book?> GetBookByNameAsync(string bookName);  
    Task AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(int bookId);
}
