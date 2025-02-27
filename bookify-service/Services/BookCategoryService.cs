using AutoMapper;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
using bookify_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class BookCategoryService : IBookCategoryService
    {
        private readonly IBookCategoryRepository _bookCategoryRepository;
        private readonly IMapper _mapper;

        public BookCategoryService(IBookCategoryRepository bookCategoryRepository, IMapper mapper)
        {
            _bookCategoryRepository = bookCategoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetBookCategoryDTO>> GetAllAsync()
        {
            var bookCategoryList = await _bookCategoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetBookCategoryDTO>>(bookCategoryList);
        }
        public async Task<GetBookCategoryDTO?> GetByIdAsync(int id)
        {
            var bookCategory = await _bookCategoryRepository.GetByIdAsync(id);
            return _mapper.Map<GetBookCategoryDTO>(bookCategory);
        }
        public async Task<bool> CreateBookCategoryAsync(AddBookCategoryDTO addBookCategoryDto)
        {
            var categoryToAdd = _mapper.Map<BookCategory>(addBookCategoryDto);
            categoryToAdd.CreatedDate = DateTime.UtcNow;
            categoryToAdd.LastEdited = DateTime.UtcNow;
            categoryToAdd.Status = 1;
            return await _bookCategoryRepository.InsertAsync(categoryToAdd);
        }
        public async Task<bool> UpdateBookCategoryAsync(int id, UpdateBookCategoryDTO updateBookCategoryDto)
        {
            var bookCategory = await _bookCategoryRepository.GetByIdAsync(id);
            if (bookCategory == null)
                return false;
            if (updateBookCategoryDto.Status != 1 && updateBookCategoryDto.Status != 0)
            {
                throw new ArgumentException("Invalid Book Category");
            }
            _mapper.Map(updateBookCategoryDto, bookCategory);
            bookCategory.CreatedDate = DateTime.UtcNow;
            return await _bookCategoryRepository.UpdateAsync(bookCategory);

        }
    }
}
