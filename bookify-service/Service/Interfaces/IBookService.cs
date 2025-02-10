using bookify_api.DTOs.BookDTO;
using Microsoft.AspNetCore.Http;

namespace bookify_api.Services
{
    public interface IBookService
    {
        Task<IEnumerable<GetBooksDTO>> GetAllBooksAsync();
        Task<GetBookDTO?> GetBookByIdAsync(int bookId);
        Task AddBookAsync(AddBookDTO bookDto, IFormFile? imageFile);
        Task UpdateBookAsync(UpdateBookDTO bookDto, IFormFile? imageFile);
        Task DeleteBookAsync(int bookId);
    }
}