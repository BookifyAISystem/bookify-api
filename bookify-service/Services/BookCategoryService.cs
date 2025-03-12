using Amazon.S3.Model;
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
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public BookCategoryService(IBookCategoryRepository bookCategoryRepository, IBookRepository bookRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _bookCategoryRepository = bookCategoryRepository;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
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

        public async Task<bool> AssignCategoriesToBookAsync(int bookId, List<int> categoryIds)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null) return false;

            var currentCategories = await _bookCategoryRepository.GetByBookIdAsync(bookId);
            var currentCategoryIds = currentCategories.Select(bc => bc.CategoryId).ToList();

            // Xóa những danh mục cũ không còn trong danh sách mới
            var toRemove = currentCategories.Where(bc => !categoryIds.Contains(bc.CategoryId)).ToList();
            foreach (var item in toRemove)
                await _bookCategoryRepository.RemoveAsync(item);

            // Thêm danh mục mới
            var toAdd = categoryIds.Except(currentCategoryIds).ToList();
            foreach (var categoryId in toAdd)
            {
                var newBookCategory = new BookCategory
                {
                    BookId = bookId,
                    CategoryId = categoryId,
                    CreatedDate = DateTime.UtcNow,
                    Status = 1
                };
                await _bookCategoryRepository.InsertAsync(newBookCategory);
            }

            return true;
        }

        public async Task<List<Category?>> GetCategoriesByBookIdAsync(int bookId)
        {
            return await _bookCategoryRepository.GetByBookIdAsync(bookId)
                                          .ContinueWith(task => task.Result.Select(bc => bc.Category).ToList());
        }

        public async Task<bool> RemoveCategoryFromBookAsync(int bookId, int categoryId)
        {
            var bookCategory = (await _bookCategoryRepository.GetByBookIdAsync(bookId))
                              .FirstOrDefault(bc => bc.CategoryId == categoryId);
            if (bookCategory == null) return false;

            await _bookCategoryRepository.RemoveAsync(bookCategory);
            return true;
        }

        public async Task<bool> UpdateBookCategoryStatusAsync(int id, int newStatus)
        {
            var bookCategory = await _bookCategoryRepository.GetByIdAsync(id);
            if (bookCategory == null)
            {
                throw new Exception($"Not found BookCategory with ID = {bookCategory}");
            }

            if (bookCategory.Status != 0 && bookCategory.Status != 1 )
            {
                throw new ArgumentException("Invalid Order Status");
            }
            bookCategory.Status = newStatus;
            bookCategory.LastEdited = DateTime.UtcNow;
            return await _bookCategoryRepository.UpdateAsync(bookCategory);
        }

        public async Task<bool> DeleteBookCategoryAsync(int id)
        {
            var bookCategory = await _bookCategoryRepository.GetByIdAsync(id);
            if (bookCategory == null)
            {
                throw new Exception($"Not found BookCategory with ID = {bookCategory}");
            }

            bookCategory.Status = 0;
            bookCategory.LastEdited = DateTime.UtcNow;
            return await _bookCategoryRepository.UpdateAsync(bookCategory);
        }
    }
}
