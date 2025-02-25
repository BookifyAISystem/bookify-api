using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<GetOrderDetailDTO>> GetAllAsync();
        Task<GetOrderDetailDTO?> GetByIdAsync(int id);
        Task<bool> CreateOrderDetailAsync(AddOrderDetailDTO addOrderDetailDto);
        Task<bool> UpdateOrderDetailAsync(int id, UpdateOrderDetailDTO updateOrderDetailDto);
        Task<bool> DeleteOrderDetailAsync(int id);
    }
}
