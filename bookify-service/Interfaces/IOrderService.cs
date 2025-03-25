
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
        //Flow Admin
        Task<IEnumerable<GetOrderDTO>> GetAllOrderAsync();
        Task<IEnumerable<GetOrderDTO>> GetOrdersByStatusAsync(int status);
        Task<IEnumerable<GetOrderDTO>> GetOrdersByAccountIdAsync(int accountId);
        Task<GetOrderDTO?> GetOrderByIdAsync(int id);
        Task<Order?> GetEntitesByIdAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int orderId, int newStatus);
        Task<IEnumerable<GetOrderDetailDTO>> GetAllOrderDetailAsync();
        Task<GetOrderDetailDTO?> GetOrderDetailByIdAsync(int id);
        Task<bool> UpdateOrderDetailStatusAsync(int id, int newStatus);

        // Flow User
        Task<bool> CreateOrderAsync(AddOrderDTO addOrderDTO); // Tao gio hang trong 
        Task<bool> CreateEmptyOrderByAccountIdAsync(int accountId);
        Task<bool> AddOrderDetailAsync(int orderId, AddOrderDetailDTO addOrderDetailDto); // Them san pham vao gio hang, cap nhat thanh tien cua san pham va tong cong cua don hang
        Task<bool> UpdateOrderAsync(int id, UpdateOrderDTO updateOrderDto);
        Task<bool> UpdateOrderDetailQuantityAsync(int orderDetailId, int quantity); // Cap nhat so luong san pham trong gio hang, cap nhat thanh tien cua san pham va tong cong cua don hang
        Task<bool> RemoveOrderDetailAsync(int orderDetailId); // Xoa san pham khoi gio hang, cap nhat tong cong cua don hang
        Task<bool> ConfirmOrderAsync(int orderId, int voucher); // Xac nhan don hang, cap nhat trang thai don hang, cap nhat trang thai san pham trong don hang
        Task<bool> CancelOrderAsync(int orderId, string cancelReason); // Huy don hang, cap nhat trang thai don hang, cap nhat trang thai san pham trong don hang
         
        // Feedback check order Detail
        Task<bool> UpdateOrderDetailsStatusAsync(int id, int newStatus);
    }
}
