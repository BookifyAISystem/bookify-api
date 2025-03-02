using bookify_data.DTOs.BookAuthorDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IBookAuthorService
    {
        Task<IEnumerable<GetBookAuthorDTO>> GetAllBookAuthorsAsync();
        Task<GetBookAuthorDTO?> GetBookAuthorByIdAsync(int bookAuthorId);
        Task AddBookAuthorAsync(CreateBookAuthorDTO bookAuthorDto);
        Task UpdateBookAuthorAsync(UpdateBookAuthorDTO bookAuthorDto);
        Task DeleteBookAuthorAsync(int bookAuthorId);
        Task UpdateStatusAsync(int bookAuthorId, int status);

    }
}
