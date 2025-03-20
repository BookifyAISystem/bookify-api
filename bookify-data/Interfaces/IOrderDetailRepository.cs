using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<IEnumerable<OrderDetail>> GetAllAsync();
        Task<OrderDetail?> GetByIdAsync(int id);
        Task<IEnumerable<OrderDetail>> GetByOrderIdAsync(int orderId);
        Task<bool> InsertAsync(OrderDetail orderDetail);
        Task<bool> UpdateAsync(OrderDetail orderDetail);
        Task<bool> DeleteAsync(int orderDetailId);
    }
}
