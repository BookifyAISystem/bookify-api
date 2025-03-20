using Amazon.Runtime;
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
        private readonly AmazonS3Service _amazonS3Service;  // ✅ Thêm dịch vụ lưu trữ ảnh

        public AuthorService(IAuthorRepository authorRepository, AmazonS3Service amazonS3Service)
        {
            _authorRepository = authorRepository;
            _amazonS3Service = amazonS3Service;
        }

        /// <summary>
        /// Lấy danh sách tất cả tác giả
        /// </summary>
        public async Task<IEnumerable<GetAuthorDTO>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepository.GetAllAuthorsAsync();
            return authors.Select(a => new GetAuthorDTO
            {
                AuthorId = a.AuthorId,
                AuthorName = a.AuthorName,
                Content = a.Content,
                AuthorImage = a.AuthorImage, // ✅ Trả về đường dẫn ảnh
                CreatedDate = a.CreatedDate.AddHours(7),
                LastEdited = a.LastEdited.AddHours(7),
                Status = a.Status
            }).ToList();
        }

        /// <summary>
        /// Lấy thông tin tác giả theo ID
        /// </summary>
        public async Task<GetAuthorDTO?> GetAuthorByIdAsync(int authorId)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(authorId);
            if (author == null) return null;

            return new GetAuthorDTO
            {
                AuthorId = author.AuthorId,
                AuthorName = author.AuthorName,
                Content = author.Content,
                AuthorImage = author.AuthorImage, // ✅ Trả về hình ảnh
                CreatedDate = author.CreatedDate.AddHours(7),
                LastEdited = author.LastEdited.AddHours(7),
                Status = author.Status
            };
        }

        /// <summary>
        /// Thêm mới tác giả với hỗ trợ upload ảnh
        /// </summary>
        public async Task AddAuthorAsync(CreateAuthorDTO authorDto)
        {
            var author = new Author
            {
                AuthorName = authorDto.AuthorName,
                Content = authorDto.Content,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                LastEdited = DateTime.UtcNow.AddHours(7),
                Status = 1
            };

            // ✅ Xử lý upload ảnh nếu có
            if (authorDto.AuthorImageFile != null)
            {
                using var stream = authorDto.AuthorImageFile.OpenReadStream();
                author.AuthorImage = await _amazonS3Service.UploadFileAsync(stream, authorDto.AuthorImageFile.FileName, authorDto.AuthorImageFile.ContentType);
            }

            await _authorRepository.AddAuthorAsync(author);
        }

        /// <summary>
        /// Cập nhật thông tin tác giả, hỗ trợ cập nhật ảnh
        /// </summary>
        public async Task UpdateAuthorAsync(UpdateAuthorDTO authorDto)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(authorDto.AuthorId);
            if (author == null)
            {
                throw new KeyNotFoundException($"Author với ID {authorDto.AuthorId} không tồn tại.");
            }

            author.AuthorName = authorDto.AuthorName ?? author.AuthorName;
            author.Content = authorDto.Content ?? author.Content;
            author.Status = authorDto.Status;
            author.LastEdited = DateTime.UtcNow.AddHours(7);

            // ✅ Xử lý thay đổi ảnh nếu có ảnh mới
            if (authorDto.AuthorImageFile != null)
            {
                using var stream = authorDto.AuthorImageFile.OpenReadStream();
                author.AuthorImage = await _amazonS3Service.UploadFileAsync(stream, authorDto.AuthorImageFile.FileName, authorDto.AuthorImageFile.ContentType);
            }

            await _authorRepository.UpdateAuthorAsync(author);
        }

        /// <summary>
        /// Xóa tác giả (soft delete)
        /// </summary>
        public async Task DeleteAuthorAsync(int authorId)
        {
            await _authorRepository.DeleteAuthorAsync(authorId);
        }

        /// <summary>
        /// Cập nhật trạng thái tác giả
        /// </summary>
        public async Task UpdateStatusAsync(int authorId, int status)
        {
            await _authorRepository.UpdateStatusAsync(authorId, status);
        }
    }
}
