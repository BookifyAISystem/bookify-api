using bookify_data.Model.OrderModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IOrderServices
    {
        Task<IEnumerable<GetOrdersDTO>> GetAllOrdersAsync();
        Task<GetOrderDTO?> GetOrderByIdAsync(int orderId);
        Task AddOrderAsync(AddOrderDTO orderDto);
        Task UpdateOrderAsync(UpdateOrderDTO orderDto);
        Task DeleteOrderAsync(int orderId);
    }
}
