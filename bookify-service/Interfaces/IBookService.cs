using bookify_data.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IBookService
    {
        Task<(IEnumerable<GetBookDTO> Books, int TotalCount)> GetAllBooksAsync(int pageNumber, int pageSize);
        Task<GetBookDTO?> GetBookByIdAsync(int bookId);
        Task AddBookAsync(AddBookDTO bookDto);
        Task UpdateBookAsync(UpdateBookDTO bookDto);
        Task DeleteBookAsync(int bookId);
        Task<(IEnumerable<GetBookDTO>, int)> SearchBooksAsync(string query, int pageNumber, int pageSize);

    }
}
