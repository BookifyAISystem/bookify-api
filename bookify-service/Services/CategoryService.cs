using AutoMapper;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
using bookify_data.Repository;
using bookify_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService( ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCategoryDTO>> GetAllAsync()
        {
            var categoryList = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetCategoryDTO>>(categoryList);
        }
        public async Task<GetCategoryDTO?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<GetCategoryDTO>(category);
        }
        public async Task<bool> CreateCategoryAsync(AddCategoryDTO addCategoryDto)
        {
            var categoryToAdd = _mapper.Map<Category>(addCategoryDto);
            categoryToAdd.CreatedDate = DateTime.UtcNow;
            categoryToAdd.LastEdited = DateTime.UtcNow;
            categoryToAdd.Status = 1;
            return await _categoryRepository.InsertAsync(categoryToAdd);
        }
        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDTO updateCategoryDto)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;
            if (updateCategoryDto.Status != 1 && updateCategoryDto.Status != 0)
            {
                throw new ArgumentException("Invalid category");
            }  
            _mapper.Map(updateCategoryDto, category);
            category.CreatedDate = DateTime.UtcNow;
            return await _categoryRepository.UpdateAsync(category);

        }
    }
}
