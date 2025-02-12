using bookify_api.DTOs.BookDTO;
using bookify_api.Repositories;
using bookify_api.Services;
using bookify_data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;
using bookify_api.Validators;
using bookify_service.Interfaces;

namespace bookify_api.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IWebHostEnvironment _env;
        private readonly BookValidator _bookValidator;

        public BookService(IBookRepository bookRepository, BookValidator bookValidator)
        {
            _bookRepository = bookRepository;
            _env = env;
            _bookValidator = bookValidator;
        }

        public async Task<IEnumerable<GetBooksDTO>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<GetBookDTO?> GetBookByIdAsync(int bookId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {bookId} was not found.");
            }

            return book;
        }

        public async Task AddBookAsync(AddBookDTO bookDto, IFormFile? imageFile)
        {
            if (bookDto == null)
            {
                throw new ArgumentNullException(nameof(bookDto), "Book data cannot be null.");
            }

            await _bookValidator.ValidateAsync(bookDto);

            var book = new Book
            {
                BookName = bookDto.BookName,
                BookType = bookDto.BookType,
                Price = bookDto.Price,
                Description = bookDto.Description,
                PulishYear = bookDto.PulishYear,
                AuthorId = bookDto.AuthorId,
                CategoryId = bookDto.CategoryId,
                CollectionId = bookDto.CollectionId,
                PromotionId = bookDto.PromotionId
            };

            if (imageFile != null)
            {
                try
                {
                    string filePath = await SaveImageAsync(imageFile);
                    book.BookImage = filePath;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to save the image file.", ex);
                }
            }

            await _bookRepository.AddBookAsync(book);
        }

        public async Task UpdateBookAsync(UpdateBookDTO bookDto, IFormFile? imageFile)
        {
            var book = await _bookRepository.GetBookEntityByIdAsync(bookDto.BookId);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {bookDto.BookId} was not found.");
            }

            await _bookValidator.ValidateAsync(bookDto);

            book.BookName = bookDto.BookName;
            book.BookType = bookDto.BookType;
            book.Price = bookDto.Price;
            book.Description = bookDto.Description;
            book.PulishYear = bookDto.PulishYear;
            book.AuthorId = bookDto.AuthorId;
            book.CategoryId = bookDto.CategoryId;
            book.CollectionId = bookDto.CollectionId;
            book.PromotionId = bookDto.PromotionId;

            if (imageFile != null)
            {
                try
                {
                    string filePath = await SaveImageAsync(imageFile);
                    book.BookImage = filePath;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Failed to update the image file.", ex);
                }
            }

            await _bookRepository.UpdateBookAsync(book);
        }

        public async Task DeleteBookAsync(int bookId)
        {
            var book = await _bookRepository.GetBookEntityByIdAsync(bookId);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {bookId} was not found.");
            }

            await _bookRepository.DeleteBookAsync(bookId);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "books");
            Directory.CreateDirectory(uploadsFolder);

            string fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return $"/images/books/{fileName}";
        }
    }
}