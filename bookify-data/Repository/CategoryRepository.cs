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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookifyDbContext _context;
        public CategoryRepository(BookifyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(o => o.CategoryId == id);
        }
        public async Task<bool> InsertAsync(Category category)
        {

            await _context.Categories.AddAsync(category);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<bool> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
