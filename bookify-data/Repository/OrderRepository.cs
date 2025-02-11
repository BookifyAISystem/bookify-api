using bookify_data.Data;
using bookify_data.Model.OrderModel;
using Microsoft.EntityFrameworkCore;
using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bookify_data.Interfaces;

namespace bookify_data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookifyDbContext _dbContext;

        public OrderRepository(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GetOrdersDTO>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders
                .Select(order => new GetOrdersDTO
                {

                    OrderId = order.OrderId,
                    Total = order.Total,
                    CreateDate = order.CreateDate,
                    Status = order.Status,
                    CancelReason = order.CancelReason,
                })
                .ToListAsync();
        }
        public async Task<GetOrderDTO?> GetOrderByIdAsync(int orderId)
        {
            return await _dbContext.Orders
                .Where(order => order.OrderId == orderId)
                .Select(order => new GetOrderDTO
                {
                    OrderId = order.OrderId,
                    Total = order.Total,
                    CreateDate = order.CreateDate,
                    Status = order.Status,
                    CancelReason = order.CancelReason,
                })
                .FirstOrDefaultAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<Order?> GetOrderEntityByIdAsync(int orderId)
        {
            return await _dbContext.Orders.FirstOrDefaultAsync(b => b.OrderId == orderId);
        }
    }
}
