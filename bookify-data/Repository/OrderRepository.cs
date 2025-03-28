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

        public async Task<IEnumerable<Order>> GetOrdersByAccountIdAsync(int accountId)
        {
            return await _context.Orders
                .Where(o => o.AccountId == accountId)
                .Include(o => o.OrderDetails)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatus(int status)
        {
            return await _context.Orders
                .Where(o => o.Status == status)
                .Include(o => o.OrderDetails)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetByAccountIdAsync(int accountId) 
        {
            return await _context.Orders
                .Where(n => n.AccountId == accountId && n.Status != 0)
                .ToListAsync();
        }



        public void Insert(Order order)
        {
            _context.Orders.AddAsync(order);
        }
        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public async Task<bool> HasCompletedOrderForBookAsync(int accountId, int bookId)
        {
            return await _context.Orders
                .Where(o => o.AccountId == accountId && o.Status == 3)
                .AnyAsync(o => o.OrderDetails.Any(od => od.BookId == bookId));
        }

        
    }
}
