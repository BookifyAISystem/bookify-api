using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Repository
{
    public class NewsRepository : INewsRepository
    {
        private readonly BookifyDbContext _dbContext;

        public NewsRepository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<News>> GetAllAsync()
        {
            return await _dbContext.News
                .Where(r => r.Status != 0)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<News?> GetByIdAsync(int id)
        {
            return await _dbContext.News
                .Where(n => n.Status != 0)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NewsId == id);
        }

        public async Task<IEnumerable<News>> GetByAccountIdAsync(int accountId)
        {
            return await _dbContext.News
                .Where(n => n.AccountId == accountId && n.Status != 0)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(News news)
        {
            await _dbContext.News.AddAsync(news);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(News news)
        {
            _dbContext.News.Update(news);
            await _dbContext.SaveChangesAsync();
        }
    }
}
