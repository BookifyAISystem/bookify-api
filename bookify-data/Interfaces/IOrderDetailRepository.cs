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
        void Insert(OrderDetail orderDetail);
        void Update(OrderDetail orderDetail);
        void Remove(OrderDetail orderDetail);
        void Detach(OrderDetail orderDetail);
        void Attach(OrderDetail orderDetail);
    }
}
