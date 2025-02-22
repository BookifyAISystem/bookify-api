using bookify_data.Entities;
using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface INoteService
    {
        Task<IEnumerable<NoteModel>> GetAllAsync();
        Task<NoteModel?> GetByIdAsync(int id);
        Task<IEnumerable<NoteModel?>> GetByAccountIdAsync(int accountId);
        Task<Note> CreateAsync(string? content, int status, int accountId);
        Task<Note> UpdateAsync(int id, string? content, int status);
        Task<bool> DeleteAsync(int id);
        Task<bool> ChangeStatus(int id, int status);
    }
}
