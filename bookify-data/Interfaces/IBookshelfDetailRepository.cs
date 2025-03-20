using bookify_data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface IBookshelfDetailRepository
    {
        Task<IEnumerable<BookshelfDetail>> GetAllBookshelfDetailsAsync();
        Task<BookshelfDetail?> GetBookshelfDetailByIdAsync(int id);
        Task AddBookshelfDetailAsync(BookshelfDetail bookshelfDetail);
        Task UpdateBookshelfDetailAsync(BookshelfDetail bookshelfDetail);
        Task DeleteBookshelfDetailAsync(int id);
        Task UpdateStatusAsync(int bookshelfDetailId, int status);

    }
}
