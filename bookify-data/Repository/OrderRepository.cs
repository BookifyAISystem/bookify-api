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
    public class OrderRepository : IOrderRepository
    {
        private readonly BookifyDbContext _context;
        public OrderRepository(BookifyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.AsNoTracking().ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int accountId) 
        {
            return await _context.Orders
                .Where(n => n.AccountId == accountId && n.Status != 0)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> InsertAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0; 
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            _context.Orders.Remove(order);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
