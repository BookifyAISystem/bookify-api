using bookify_data.DTOs.BookAuthorDTO;
using bookify_data.Interfaces;
using bookify_service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class BookAuthorService : IBookAuthorService
    {
        private readonly IBookAuthorRepository _bookAuthorRepository;

        public BookAuthorService(IBookAuthorRepository bookAuthorRepository)
        {
            _bookAuthorRepository = bookAuthorRepository;
        }

        public async Task<IEnumerable<GetBookAuthorDTO>> GetAllBookAuthorsAsync()
        {
            return await _bookAuthorRepository.GetAllBookAuthorsAsync();
        }

        public async Task<GetBookAuthorDTO?> GetBookAuthorByIdAsync(int bookAuthorId)
        {
            return await _bookAuthorRepository.GetBookAuthorByIdAsync(bookAuthorId);
        }

        public async Task AddBookAuthorAsync(CreateBookAuthorDTO bookAuthorDto)
        {
            await _bookAuthorRepository.AddBookAuthorAsync(bookAuthorDto);
        }

        public async Task UpdateBookAuthorAsync(UpdateBookAuthorDTO bookAuthorDto)
        {
            await _bookAuthorRepository.UpdateBookAuthorAsync(bookAuthorDto);
        }

        public async Task DeleteBookAuthorAsync(int bookAuthorId)
        {
            await _bookAuthorRepository.DeleteBookAuthorAsync(bookAuthorId);
        }
        public async Task UpdateStatusAsync(int bookAuthorId, int status)
        {
            await _bookAuthorRepository.UpdateStatusAsync(bookAuthorId, status);
        }

    }
}
