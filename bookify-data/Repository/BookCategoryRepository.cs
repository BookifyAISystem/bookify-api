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
    public class BookCategoryRepository : IBookCategoryRepository
    {
        private readonly BookifyDbContext _context;
        public BookCategoryRepository(BookifyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BookCategory>> GetAllAsync()
        {
            return await _context.BookCategories.AsNoTracking().ToListAsync();
        }
        public async Task<BookCategory?> GetByIdAsync(int id)
        {
            return await _context.BookCategories.FirstOrDefaultAsync(o => o.CategoryId == id);
        }
        public async Task<bool> InsertAsync(BookCategory bookCategory)
        {

            await _context.BookCategories.AddAsync(bookCategory);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<bool> UpdateAsync(BookCategory bookCategory)
        {
            _context.BookCategories.Update(bookCategory);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
