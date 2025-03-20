using bookify_data.DTOs;
using bookify_data.DTOs.BookshelfDTO;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class BookshelfService : IBookshelfService
    {
        private readonly IBookshelfRepository _bookshelfRepository;

        public BookshelfService(IBookshelfRepository bookshelfRepository)
        {
            _bookshelfRepository = bookshelfRepository;
        }

        public async Task<IEnumerable<GetBookshelfDTO>> GetAllBookshelvesAsync()
        {
            var bookshelves = await _bookshelfRepository.GetAllBookshelvesAsync();
            return bookshelves.Select(bs => new GetBookshelfDTO
            {
                BookshelfId = bs.BookshelfId,
                AccountId = bs.AccountId,
                BookShelfName = bs.BookShelfName,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                LastEdited = DateTime.UtcNow.AddHours(7),
                Status = bs.Status
            }).ToList();
        }

        public async Task<GetBookshelfDTO?> GetBookshelfByIdAsync(int bookshelfId)
        {
            var bs = await _bookshelfRepository.GetBookshelfByIdAsync(bookshelfId);
            return bs == null ? null : new GetBookshelfDTO
            {
                BookshelfId = bs.BookshelfId,
				AccountId = bs.AccountId,
                BookShelfName = bs.BookShelfName,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                LastEdited = DateTime.UtcNow.AddHours(7),
                Status = bs.Status
            };
        }

        public async Task AddBookshelfAsync(CreateBookshelfDTO bookshelfDto)
        {
            var bookshelf = new Bookshelf
            {
				AccountId = bookshelfDto.AccountId,
                BookShelfName = bookshelfDto.BookShelfName,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                LastEdited = DateTime.UtcNow.AddHours(7),
                Status = 1
            };

            await _bookshelfRepository.AddBookshelfAsync(bookshelf);
        }

        public async Task UpdateBookshelfAsync(UpdateBookshelfDTO bookshelfDto)
        {
            var bookshelf = await _bookshelfRepository.GetBookshelfByIdAsync(bookshelfDto.BookshelfId);
            if (bookshelf == null) throw new KeyNotFoundException("Bookshelf not found.");

            bookshelf.BookShelfName = bookshelfDto.BookShelfName;
            bookshelf.LastEdited = DateTime.UtcNow.AddHours(7);

            await _bookshelfRepository.UpdateBookshelfAsync(bookshelf);
        }

        public async Task DeleteBookshelfAsync(int bookshelfId)
        {
            await _bookshelfRepository.DeleteBookshelfAsync(bookshelfId);
        }
        public async Task UpdateStatusAsync(int bookshelfId, int status)
        {
            await _bookshelfRepository.UpdateStatusAsync(bookshelfId, status);
        }

    }
}
