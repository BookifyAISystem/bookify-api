using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface INewsRepository
    {
        Task<IEnumerable<News>> GetAllAsync();
        Task<News?> GetByIdAsync(int id);
        Task<IEnumerable<News>> GetByAccountIdAsync(int accountId);
        Task AddAsync(News news);
        Task UpdateAsync(News news);
    }
}
