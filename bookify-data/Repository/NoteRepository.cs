using bookify_data.Data;
using bookify_data.Entities;
using bookify_data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly BookifyDbContext _dbContext;

        public NoteRepository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            return await _dbContext.Notes
                .Where(r => r.Status != 0)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            return await _dbContext.Notes
                .Where(n => n.Status != 0)
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.NoteId == id);
        }

        public async Task<IEnumerable<Note>> GetByAccountIdAsync(int accountId)
        {
            return await _dbContext.Notes
                .Where(n => n.AccountId == accountId && n.Status != 0)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Note note)
        {
            await _dbContext.Notes.AddAsync(note);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Note note)
        {
            _dbContext.Notes.Update(note);
            await _dbContext.SaveChangesAsync();
        }

    }
}
