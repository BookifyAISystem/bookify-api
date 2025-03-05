using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface IBookCategoryRepository
    {
        Task<IEnumerable<BookCategory>> GetAllAsync();
        Task<BookCategory?> GetByIdAsync(int id);
        Task<bool> InsertAsync(BookCategory bookCategory);
        Task<bool> UpdateAsync(BookCategory bookCategory);
        Task<List<BookCategory>> GetByBookIdAsync(int bookId);
    }
}
