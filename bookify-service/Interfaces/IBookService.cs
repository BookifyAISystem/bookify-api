using bookify_data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<GetBookDTO>> GetAllBooksAsync();
        Task<GetBookDTO?> GetBookByIdAsync(int bookId);
        Task AddBookAsync(AddBookDTO bookDto);
        Task UpdateBookAsync(UpdateBookDTO bookDto);
        Task DeleteBookAsync(int bookId);
    }
}
