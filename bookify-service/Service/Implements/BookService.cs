using bookify_api.DTOs.BookDTO;
using bookify_api.Repositories;
using bookify_api.Services;
using bookify_data.Entities;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace bookify_api.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<GetBooksDTO>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllBooksAsync();
        }

        public async Task<GetBookDTO?> GetBookByIdAsync(int bookId)
        {
            return await _bookRepository.GetBookByIdAsync(bookId);
        }

        public async Task AddBookAsync(AddBookDTO bookDto, IFormFile? imageFile)
        {
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
                string filePath = await SaveImageAsync(imageFile);
                book.BookImage = filePath;
            }

            await _bookRepository.AddBookAsync(book);
        }

        // BookService.cs
        public async Task UpdateBookAsync(UpdateBookDTO bookDto, IFormFile? imageFile)
        {
            // Gọi repository để lấy entity Book
            var book = await _bookRepository.GetBookEntityByIdAsync(bookDto.BookId);
            if (book != null)
            {
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
                    string filePath = await SaveImageAsync(imageFile);
                    book.BookImage = filePath;
                }

                await _bookRepository.UpdateBookAsync(book);
            }
        }



        public async Task DeleteBookAsync(int bookId)
        {
            await _bookRepository.DeleteBookAsync(bookId);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine("wwwroot", "images", "books");
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
