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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly BookifyDbContext _context;
        public PaymentRepository(BookifyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                         .AsNoTracking()
                         .ToListAsync();

        }

        public async Task<IEnumerable<Payment>> GetPaymentsByAccountIdAsync(int accountId)
        {
            return await _context.Payments
                .Where(o => o.Order.AccountId == accountId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatus(int status)
        {
            return await _context.Payments
                .Where(o => o.Status == status)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id) 
        {
            return await _context.Payments
                .FirstOrDefaultAsync(o => o.PaymentId == id);
        }

        public void Insert(Payment payment)
        {
            _context.Payments.AddAsync(payment);
        }
    }
}
