
using bookify_data.Entities;
using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<GetOrderDTO>> GetAllAsync();
        Task<IEnumerable<GetOrderDTO>> GetOrdersByAccountIdAsync(int accountId);
        Task<GetOrderDTO?> GetByIdAsync(int id);
        Task<Order?> GetEntitesByIdAsync(int id);
        Task<bool> CreateOrderAsync(AddOrderDTO addOrderDto);
        Task<bool> UpdateOrderAsync(int id, UpdateOrderDTO updateOrderDto);
        Task<bool> UpdateOrderStatusAsync(int orderId, int newStatus);
        Task<bool> DeleteOrderAsync(int id);
    }
}
