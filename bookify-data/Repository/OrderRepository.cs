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
            return await _context.Orders
                         .Include(o => o.OrderDetails)
                         .AsNoTracking()
                         .ToListAsync();

        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.OrderDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId) 
        {
            return await _context.Orders
                .Where(n => n.CustomerId == customerId && n.Status != 0)
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
        
    }
}
