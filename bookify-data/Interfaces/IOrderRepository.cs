using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bookify_data.Model;

namespace bookify_data.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<GetOrdersDTO>> GetAllOrdersAsync();
        Task<GetOrderDTO?> GetOrderByIdAsync(int orderId);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int orderId);
        Task<Order?> GetOrderEntityByIdAsync(int orderId);  
    }
}
