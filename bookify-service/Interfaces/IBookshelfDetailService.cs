using bookify_data.DTOs.BookshelfDetailDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IBookshelfDetailService
    {
        Task<IEnumerable<GetBookshelfDetailDTO>> GetAllBookshelfDetailsAsync();
        Task<GetBookshelfDetailDTO?> GetBookshelfDetailByIdAsync(int id);
        Task AddBookshelfDetailAsync(AddBookshelfDetailDTO bookshelfDetailDto);
        Task UpdateBookshelfDetailAsync(UpdateBookshelfDetailDTO bookshelfDetailDto);
        Task DeleteBookshelfDetailAsync(int id);
        Task UpdateStatusAsync(int bookshelfDetailId, int status);

    }
}
