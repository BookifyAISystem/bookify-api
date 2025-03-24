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

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByBookIdAndAccountId(int bookId, int accountId)
        {
            return await _context.OrderDetails
                .Include(od => od.Order)
                .Where(od => od.BookId == bookId && od.Order.AccountId == accountId )
                .ToListAsync();
        }
        public async Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderDetails
                .Where(n => n.OrderId == orderId && n.Status != 0)
                .AsNoTracking()
                .ToListAsync();
        }
        public void Insert(OrderDetail orderDetail)
        {
            _context.OrderDetails.Add(orderDetail);
        }

        public void Update(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
        }

        public void Remove(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
        }
        public void Detach(OrderDetail orderDetail)
        {
            var entry = _context.Entry(orderDetail);
            if (entry != null)
            {
                entry.State = EntityState.Detached;
            }
        }

        public void Attach(OrderDetail orderDetail)
        {
            var entry = _context.Entry(orderDetail);
            if (entry != null)
            {
                entry.State = EntityState.Modified;
            }
        }

    }
}
