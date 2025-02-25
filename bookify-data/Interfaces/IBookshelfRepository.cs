using bookify_data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface IBookshelfRepository
    {
        Task<IEnumerable<Bookshelf>> GetAllBookshelvesAsync();
        Task<Bookshelf?> GetBookshelfByIdAsync(int bookshelfId);
        Task AddBookshelfAsync(Bookshelf bookshelf);
        Task UpdateBookshelfAsync(Bookshelf bookshelf);
        Task DeleteBookshelfAsync(int bookshelfId);
    }
}
