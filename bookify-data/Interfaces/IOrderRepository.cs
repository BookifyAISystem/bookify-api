using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetOrdersByAccountIdAsync(int accountId);
        Task<IEnumerable<Order>> GetOrdersByStatus(int status);
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetByAccountIdAsync(int accountId);
        Task<bool> InsertAsync(Order order);
        Task<bool> UpdateAsync(Order order);
        Task<bool> HasCompletedOrderForBookAsync(int customerId, int bookId);
    }
}
