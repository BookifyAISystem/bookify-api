using bookify_data.Entities;
using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IBookCategoryService
    {
        Task<IEnumerable<GetBookCategoryDTO>> GetAllAsync();
        Task<GetBookCategoryDTO?> GetByIdAsync(int id);
        Task<bool> CreateBookCategoryAsync(AddBookCategoryDTO addBookCategoryDto);
        Task<bool> UpdateBookCategoryAsync(int id, UpdateBookCategoryDTO updateBookCategoryDto);
        Task<bool> AssignCategoriesToBookAsync(int bookId, List<int> categoryIds);
        Task<List<Category?>> GetCategoriesByBookIdAsync(int bookId);
        Task<bool> RemoveCategoryFromBookAsync(int bookId, int categoryId);

    }
}
