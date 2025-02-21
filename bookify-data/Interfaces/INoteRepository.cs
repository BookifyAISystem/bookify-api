using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllAsync();
        Task<Note?> GetByIdAsync(int id);
        Task<IEnumerable<Note>> GetByAccountIdAsync(int accountId);
        Task AddAsync(Note note);
        Task UpdateAsync(Note note);
    }
}
