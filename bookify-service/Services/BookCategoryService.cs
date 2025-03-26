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
        private readonly IUnitOfWork _unitOfWork;

        public BookCategoryService(IBookCategoryRepository bookCategoryRepository, IBookRepository bookRepository, ICategoryRepository categoryRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _bookCategoryRepository = bookCategoryRepository;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
            _bookCategoryRepository.InsertAsync(categoryToAdd);
            return await _unitOfWork.CompleteAsync();
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
            _bookCategoryRepository.UpdateAsync(bookCategory);
            return await _unitOfWork.CompleteAsync();

        }

        public async Task<bool> AssignCategoriyToBookAsync(int bookId, int categoryId)
        {
            var book = await _unitOfWork.Books.GetBookByIdAsync(bookId);
            if (book == null)
            {
                throw new Exception($"Book not found with ID = {bookId}");
            }
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception($"Category not found with ID = {categoryId}");
            }
            var existingBookCategory = book.BookCategories.FirstOrDefault(bc => bc.CategoryId == categoryId);
            if (existingBookCategory != null)
            {
                throw new Exception($"Failed to assign. Book with ID = {bookId} already has this category !");
            }
            var bookCategory = new BookCategory
            {
                BookId = bookId,
                CategoryId = categoryId,
                CreatedDate = DateTime.UtcNow,
                Status = 1
            };
            book.BookCategories.Add(bookCategory);
            _unitOfWork.Books.UpdateBook(book);
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> AssignCategoriesToBookAsync(int bookId, List<int> categoryIds)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null)
            {
                throw new Exception($"Book not found with ID = {bookId}");
            }

            var currentCategories = await _bookCategoryRepository.GetByBookIdAsync(bookId);
            var currentCategoryIds = currentCategories.Select(bc => bc.CategoryId).ToList();

            // Remove old categories that are not in the new list
            var toRemove = currentCategories.Where(bc => !categoryIds.Contains(bc.CategoryId)).ToList();
            foreach (var item in toRemove)
            {
                book.BookCategories.Remove(item);
            }
            await _bookRepository.UpdateBookAsync(book);
            // Add new categories that are not already assigned
            var toAdd = categoryIds.Except(currentCategoryIds).ToList();
            foreach (var categoryId in toAdd)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    throw new Exception($"Category not found with ID = {categoryId}");
                }

                var newBookCategory = new BookCategory
                {
                    BookId = bookId,
                    CategoryId = categoryId,
                    CreatedDate = DateTime.UtcNow,
                    Status = 1
                };
                book.BookCategories.Add(newBookCategory);
            }
            _bookRepository.UpdateBook(book);
            return await _unitOfWork.CompleteAsync();
        }
        public async Task<List<Category?>> GetCategoriesByBookIdAsync(int bookId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null)
            {
                throw new Exception($"Book not found with ID = {bookId}");
            }

            var bookCategories = await _bookCategoryRepository.GetByBookIdAsync(bookId);
            return bookCategories.Select(bc => bc.Category).ToList();
        }
        public async Task<bool> RemoveCategoryFromBookAsync(int bookId, int categoryId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null)
            {
                throw new Exception($"Book not found with ID = {bookId}");
            }

            var bookCategory = book.BookCategories.FirstOrDefault(bc => bc.CategoryId == categoryId);
            if (bookCategory == null)
            {
                throw new Exception($"Category not found with ID = {categoryId} for the specified book.");
            }

            await _bookCategoryRepository.RemoveAsync(bookCategory);
            _bookRepository.UpdateBook(book);
            return await _unitOfWork.CompleteAsync();
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
            _bookCategoryRepository.UpdateAsync(bookCategory);
            return await _unitOfWork.CompleteAsync();
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
            _bookCategoryRepository.UpdateAsync(bookCategory);
            return await _unitOfWork.CompleteAsync();
        }
  }
}
