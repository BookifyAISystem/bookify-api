using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<GetCategoryDTO>> GetAllAsync();
        Task<GetCategoryDTO?> GetByIdAsync(int id);
        Task<bool> CreateCategoryAsync(AddCategoryDTO addCategoryDto);
        Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDTO updateCategoryDto);
    }
}
