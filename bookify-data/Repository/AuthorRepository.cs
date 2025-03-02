using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookify_data.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookifyDbContext _dbContext;

        public AuthorRepository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            return await _dbContext.Authors
                .Where(a => a.Status == 1) // Chỉ lấy những author có status = 1 (active)
                .ToListAsync();
        }

        public async Task<Author?> GetAuthorByIdAsync(int authorId)
        {
            return await _dbContext.Authors
                .FirstOrDefaultAsync(a => a.AuthorId == authorId);
        }

        public async Task AddAuthorAsync(Author author)
        {
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAuthorAsync(Author author)
        {
            _dbContext.Authors.Update(author);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAuthorAsync(int authorId)
        {
            var author = await _dbContext.Authors.FindAsync(authorId);
            if (author != null)
            {
                // Soft delete - cập nhật status thay vì xóa vĩnh viễn
                author.Status = 0;
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task UpdateStatusAsync(int authorId, int status)
        {
            var author = await _dbContext.Authors.FindAsync(authorId);
            if (author != null)
            {
                author.Status = status;
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
