using bookify_data.DTOs.BookshelfDetailDTO;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class BookshelfDetailService : IBookshelfDetailService
    {
        private readonly IBookshelfDetailRepository _bookshelfDetailRepository;

        public BookshelfDetailService(IBookshelfDetailRepository bookshelfDetailRepository)
        {
            _bookshelfDetailRepository = bookshelfDetailRepository;
        }

        public async Task<IEnumerable<GetBookshelfDetailDTO>> GetAllBookshelfDetailsAsync()
        {
            var bookshelfDetails = await _bookshelfDetailRepository.GetAllBookshelfDetailsAsync();
            return bookshelfDetails.Select(bsd => new GetBookshelfDetailDTO
            {
                BookshelfDetailId = bsd.BookshelfDetailId,
                BookshelfId = bsd.BookshelfId,
                BookId = bsd.BookId,
                CreatedDate = bsd.CreatedDate,
                LastEdited = bsd.LastEdited,
                Status = bsd.Status
            }).ToList();
        }

        public async Task<GetBookshelfDetailDTO?> GetBookshelfDetailByIdAsync(int id)
        {
            var bsd = await _bookshelfDetailRepository.GetBookshelfDetailByIdAsync(id);
            if (bsd == null) return null;

            return new GetBookshelfDetailDTO
            {
                BookshelfDetailId = bsd.BookshelfDetailId,
                BookshelfId = bsd.BookshelfId,
                BookId = bsd.BookId,
                CreatedDate = bsd.CreatedDate,
                LastEdited = bsd.LastEdited,
                Status = bsd.Status
            };
        }

        public async Task AddBookshelfDetailAsync(AddBookshelfDetailDTO bookshelfDetailDto)
        {
            var bookshelfDetail = new BookshelfDetail
            {
                BookshelfId = bookshelfDetailDto.BookshelfId,
                BookId = bookshelfDetailDto.BookId,
                CreatedDate = DateTime.UtcNow,
                LastEdited = DateTime.UtcNow,
                Status = 1
            };

            await _bookshelfDetailRepository.AddBookshelfDetailAsync(bookshelfDetail);
        }

        public async Task UpdateBookshelfDetailAsync(UpdateBookshelfDetailDTO bookshelfDetailDto)
        {
            var bookshelfDetail = await _bookshelfDetailRepository.GetBookshelfDetailByIdAsync(bookshelfDetailDto.BookshelfDetailId);
            if (bookshelfDetail == null) throw new KeyNotFoundException("Bookshelf detail not found");

            bookshelfDetail.BookshelfId = bookshelfDetailDto.BookshelfId;
            bookshelfDetail.BookId = bookshelfDetailDto.BookId;
            bookshelfDetail.LastEdited = DateTime.UtcNow;

            await _bookshelfDetailRepository.UpdateBookshelfDetailAsync(bookshelfDetail);
        }

        public async Task DeleteBookshelfDetailAsync(int id)
        {
            await _bookshelfDetailRepository.DeleteBookshelfDetailAsync(id);
        }
        public async Task UpdateStatusAsync(int bookshelfDetailId, int status)
        {
            await _bookshelfDetailRepository.UpdateStatusAsync(bookshelfDetailId, status);
        }

    }
}
