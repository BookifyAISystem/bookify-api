
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
        Task<GetOrderDTO?> GetByIdAsync(int id);
        Task<bool> CreateOrderAsync(AddOrderDTO addOrderDto);
        Task<bool> UpdateOrderAsync(int id, UpdateOrderDTO updateOrderDto);
        Task<bool> DeleteOrderAsync(int id);

    }
}
