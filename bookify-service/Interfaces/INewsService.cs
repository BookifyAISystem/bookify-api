using bookify_data.Entities;
using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsModel>> GetAllAsync();
        Task<NewsModel?> GetByIdAsync(int id);
        Task<IEnumerable<NewsModel?>> GetByAccountIdAsync(int accountId);
        Task<News> CreateAsync(string? title, string? content, string? summary, string? imageUrl, DateTime publishAt, int accountId, int status);
        Task<News> UpdateAsync(int id, string? title, string? content, string? summary, string? imageUrl, DateTime publishAt, int status);
        Task<bool> DeleteAsync(int id);
        Task<bool> ChangeStatus(int id, int status);
    }
}
