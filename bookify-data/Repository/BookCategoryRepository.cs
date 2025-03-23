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
        public void InsertAsync(BookCategory bookCategory)
        {
            _context.BookCategories.AddAsync(bookCategory);

        }
        public void UpdateAsync(BookCategory bookCategory)
        {
            _context.BookCategories.Update(bookCategory);
        }

        public async Task<List<BookCategory>> GetByBookIdAsync(int bookId)
        {
            return await _context.BookCategories.Where(bc => bc.BookId == bookId).ToListAsync();
        }

        public async Task RemoveAsync(BookCategory bookCategory)
        {
            _context.BookCategories.Remove(bookCategory);
            await _context.SaveChangesAsync();
        }
    }
}
