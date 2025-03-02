using bookify_data.DTOs.AuthorDTO;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<GetAuthorDTO>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepository.GetAllAuthorsAsync();
            return authors.Select(a => new GetAuthorDTO
            {
                AuthorId = a.AuthorId,
                AuthorName = a.AuthorName,
                Content = a.Content,
                CreatedDate = a.CreatedDate,
                LastEdited = a.LastEdited,
                Status = a.Status
            }).ToList();
        }

        public async Task<GetAuthorDTO?> GetAuthorByIdAsync(int authorId)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(authorId);
            if (author == null) return null;

            return new GetAuthorDTO
            {
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
                Content = author.Content,
                CreatedDate = author.CreatedDate,
                LastEdited = author.LastEdited,
                Status = author.Status
            };
        }

        public async Task AddAuthorAsync(CreateAuthorDTO authorDto)
        {
            var author = new Author
            {
                AuthorName = authorDto.AuthorName,
                Content = authorDto.Content,
                CreatedDate = DateTime.UtcNow,
                LastEdited = DateTime.UtcNow,
                Status = 1
            };

            await _authorRepository.AddAuthorAsync(author);
        }

        public async Task UpdateAuthorAsync(UpdateAuthorDTO authorDto)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(authorDto.AuthorId);
            if (author == null)
            {
                throw new KeyNotFoundException($"Author với ID {authorDto.AuthorId} không tồn tại.");
            }

            author.AuthorName = authorDto.AuthorName;
            author.Content = authorDto.Content;
            author.Status = authorDto.Status;
            author.LastEdited = DateTime.UtcNow;

            await _authorRepository.UpdateAuthorAsync(author);
        }

        public async Task DeleteAuthorAsync(int authorId)
        {
            await _authorRepository.DeleteAuthorAsync(authorId);
        }
        public async Task UpdateStatusAsync(int authorId, int status)
        {
            await _authorRepository.UpdateStatusAsync(authorId, status);
        }

    }
}
