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
        Task<IEnumerable<GetOrderDetailDTO>> GetAllOrderDetailAsync();
        Task<GetOrderDetailDTO?> GetByOrderDetailByIdAsync(int id);
        Task<bool> AddOrderDetailAsync(AddOrderDetailDTO addOrderDetailDto);
        Task<bool> UpdateOrderDetailAsync(int id, UpdateOrderDetailDTO updateOrderDetailDto);
        Task<bool> UpdateOrderDetailStatusAsync(int id, int newStatus);
        Task<bool> DeleteOrderDetailAsync(int id);

    }
}
