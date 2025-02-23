using bookify_data.DTOs;
using bookify_data.DTOs.BookshelfDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IBookshelfService
    {
        Task<IEnumerable<GetBookshelfDTO>> GetAllBookshelvesAsync();
        Task<GetBookshelfDTO?> GetBookshelfByIdAsync(int bookshelfId);
        Task AddBookshelfAsync(CreateBookshelfDTO bookshelfDto);
        Task UpdateBookshelfAsync(UpdateBookshelfDTO bookshelfDto);
        Task DeleteBookshelfAsync(int bookshelfId);
    }
}
