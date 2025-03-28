﻿using bookify_data.Data;
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
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly BookifyDbContext _context;
        public FeedbackRepository(BookifyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            return await _context.Feedbacks.AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<Feedback>> GetByAccountIdAndStatusAsync(int accountId, int status)
        {
            return await _context.Feedbacks.Where(o => o.AccountId == accountId && o.Status == status).ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetByBookId(int bookId)
        {
            return await _context.Feedbacks.Where(o => o.BookId == bookId && o.Status == 2).ToListAsync();
        }
        public async Task<Feedback?> GetByIdAsync(int id)
        {
            return await _context.Feedbacks.FirstOrDefaultAsync(o => o.FeedbackId == id);
        }
        public async Task<bool> InsertAsync(Feedback feedback)
        {

            await _context.Feedbacks.AddAsync(feedback);
            return await _context.SaveChangesAsync() > 0;

        }
        public async Task<bool> UpdateAsync(Feedback feedback)
        {
            _context.Feedbacks.Update(feedback);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
