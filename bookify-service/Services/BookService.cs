using bookify_data.DTOs;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly AmazonS3Service _amazonS3Service;

        public BookService(IBookRepository bookRepository, AmazonS3Service amazonS3Service)
        {
            _bookRepository = bookRepository;
            _amazonS3Service = amazonS3Service;
        }

        public async Task<(IEnumerable<GetBookDTO>, int)> GetAllBooksAsync(int pageNumber, int pageSize = 12)
        {
            var queryBooks = _bookRepository.QueryBooks()
                .Where(book => book.Status == 1); 

            int totalCount = await queryBooks.CountAsync(); 

            var books = await queryBooks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(book => new GetBookDTO
                {
                    BookId = book.BookId,
                    BookName = book.BookName,
                    BookImage = book.BookImage,
                    BookType = book.BookType,
                    Price = book.Price,
                    PriceEbook = book.PriceEbook,
                    Description = book.Description,
                    BookContent = book.BookContent,
                    PublishYear = book.PublishYear,
                    CategoryId = book.CategoryId,
                    PromotionId = book.PromotionId,
                    ParentBookId = book.ParentBookId,
                    AuthorId = book.AuthorId,
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    LastEdited = DateTime.UtcNow.AddHours(7),

                })
                .ToListAsync();

            return (books, totalCount);
        }


        public async Task<GetBookDTO?> GetBookByIdAsync(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(bookId);
                if (book == null)
                {
                    throw new KeyNotFoundException($"Book với ID {bookId} không tồn tại.");
                }

                return new GetBookDTO
                {
                    BookId = book.BookId,
                    BookName = book.BookName,
                    BookImage = book.BookImage,
                    BookType = book.BookType,
                    Price = book.Price,
                    PriceEbook = book.PriceEbook,
                    Description = book.Description,
                    BookContent = book.BookContent,
                    PublishYear = book.PublishYear,
                    CategoryId  = book.CategoryId,
                    PromotionId = book.PromotionId,
                    ParentBookId    = book.ParentBookId,
                    AuthorId = book.AuthorId,
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    LastEdited = DateTime.UtcNow.AddHours(7),
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết sách từ database", ex);
            }
        }

        public async Task AddBookAsync(AddBookDTO bookDto)
        {
            try
            {
                var book = new Book
                {
                    BookName = bookDto.BookName,
                    BookType = bookDto.BookType,
                    Price = bookDto.Price,
                    PriceEbook = bookDto.PriceEbook,
                    Description = bookDto.Description,
                    BookContent = bookDto.BookContent,
                    PublishYear = bookDto.PublishYear,
                    CategoryId = bookDto.CategoryId,
                    PromotionId = bookDto.PromotionId,
                    ParentBookId = bookDto.ParentBookId,
                    AuthorId = bookDto.AuthorId,
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    LastEdited = DateTime.UtcNow.AddHours(7),
                };

                if (bookDto.ImageFile != null)
                {
                    using var stream = bookDto.ImageFile.OpenReadStream();
                    book.BookImage = await _amazonS3Service.UploadFileAsync(stream, bookDto.ImageFile.FileName, bookDto.ImageFile.ContentType);
                }

                await _bookRepository.AddBookAsync(book);
                await _bookRepository.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }


        public async Task UpdateBookAsync(UpdateBookDTO bookDto)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(bookDto.BookId);
                if (book == null)
                {
                    throw new KeyNotFoundException($"Book với ID {bookDto.BookId} không tồn tại.");
                }

                book.BookName = bookDto.BookName;
                book.BookType = bookDto.BookType;
                book.Price = bookDto.Price;
                book.PriceEbook = bookDto.PriceEbook;
                book.Description = bookDto.Description;
                book.BookContent = bookDto.BookContent;
                book.PublishYear = bookDto.PublishYear;
                book.CategoryId = bookDto.CategoryId;
                book.PromotionId = bookDto.PromotionId;
                book.ParentBookId = bookDto.ParentBookId;
                book.AuthorId = bookDto.AuthorId;
                book.LastEdited = DateTime.UtcNow.AddHours(7);

                if (bookDto.ImageFile != null)
                {
                    // Xóa ảnh cũ nếu có
                    if (!string.IsNullOrEmpty(book.BookImage))
                    {
                        await _amazonS3Service.DeleteFileAsync(book.BookImage);
                    }

                    // Upload ảnh mới lên S3
                    using var stream = bookDto.ImageFile.OpenReadStream();
                    book.BookImage = await _amazonS3Service.UploadFileAsync(stream, bookDto.ImageFile.FileName, bookDto.ImageFile.ContentType);
                }

                await _bookRepository.UpdateBookAsync(book);
                await _bookRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sách vào database", ex);
            }
        }

        public async Task DeleteBookAsync(int bookId)
        {
            var book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book == null)
            {
                throw new KeyNotFoundException($"Book với ID {bookId} không tồn tại.");
            }

            // Xóa mềm bằng cách đặt Status = 0
            book.Status = 0;
            book.LastEdited = DateTime.UtcNow;

            await _bookRepository.UpdateBookAsync(book);
        }

        // 🔍 Gợi ý sách khi nhập ký tự
        public async Task<IEnumerable<GetBookDTO>> SuggestBooksAsync(string query, int limit)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<GetBookDTO>();
            }

            return await _bookRepository.SuggestBooksAsync(query, limit);
        }

        // 🔍 Phân trang khi tìm kiếm
        public async Task<(IEnumerable<GetBookDTO>, int)> SearchBooksAsync(string query, int pageNumber, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return (new List<GetBookDTO>(), 0);
            }

            return await _bookRepository.SearchBooksAsync(query, pageNumber, pageSize);
        }
        public async Task UpdateStatusAsync(int bookId, int status)
        {
            await _bookRepository.UpdateStatusAsync(bookId, status);
        }


    }
}
