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
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly BookifyDbContext _context;
        public OrderDetailRepository(BookifyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _context.OrderDetails.AsNoTracking().ToListAsync();
        }
        public async Task<OrderDetail?> GetByIdAsync(int id)
        {
            return await _context.OrderDetails.FirstOrDefaultAsync(o => o.OrderDetailId == id);
        }
        public async Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Where(n => n.OrderId == orderId && n.Status != 0)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> InsertAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateAsync(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
            return await _context.SaveChangesAsync() > 0;
        }
        
    }
}
