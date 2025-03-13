using AutoMapper;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
using bookify_data.Repository;
using bookify_service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IVoucherRepository voucherRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _voucherRepository = voucherRepository;
        }

        public async Task<IEnumerable<GetOrderDTO>> GetAllAsync()
        {
            var orderList = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetOrderDTO>>(orderList);

        }
        public async Task<GetOrderDTO?> GetByIdAsync(int id)
        {
            var order =  await _orderRepository.GetByIdAsync(id);
            return  _mapper.Map<GetOrderDTO>(order);
        }

        public async Task<Order?> GetEntitesByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order;
            }
        public async Task<bool> CreateOrderAsync(AddOrderDTO addOrderDto)
        {
            var order = new Order
            {
                AccountId = addOrderDto.AccountId,
                VoucherId = addOrderDto.VoucherId,
                CreatedDate = DateTime.UtcNow,
                LastEdited = DateTime.UtcNow,
                Status = 1 // active
            };
            foreach (var odDto in addOrderDto.OrderDetails )
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    Quantity = odDto.Quantity,
                    Price = odDto.Price,
                    CreatedDate = DateTime.UtcNow,
                    LastEdited = DateTime.UtcNow,
                    Status = 1
                };
                order.OrderDetails.Add(orderDetail);
            }
            int total = order.OrderDetails.Sum(x => x.Quantity * x.Price);

            if (order.VoucherId.HasValue)
            {
                var voucher = await _voucherRepository.GetByIdAsync(order.VoucherId.Value);
                if (voucher != null)
                {
                    if (total >= voucher.MinAmount && voucher.Quantity > 0)
                    { 
                    int discountValue = (int)(total * (voucher.Discount / 100.0));
                        if (discountValue > voucher.MaxDiscount)
                        {
                            discountValue = voucher.MaxDiscount;
                        }
                        total -= discountValue;
                        voucher.Quantity--;
                        voucher.LastEdited = DateTime.UtcNow;
                        await _voucherRepository.UpdateAsync(voucher);

                    }
                }
            }
            order.Total = total;

            return await _orderRepository.InsertAsync(order);

        }
        public async Task<bool> UpdateOrderAsync(int id, UpdateOrderDTO updateOrderDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return false;
            if (updateOrderDto.Status != 1 && updateOrderDto.Status != 0 && updateOrderDto.Status != 2)
            {
                throw new ArgumentException("Invalid Order");
            }
            order.Status = updateOrderDto.Status;
            if (updateOrderDto.Status == 0) // giả sử 0 = hủy
            {
                order.CancelReason = updateOrderDto.CancelReason;
            }
            order.LastEdited = DateTime.UtcNow;
            return await _orderRepository.UpdateAsync(order);
            // Truong hop bang 2 - thanh cong . Co the tao 1 DB danh cho luu tru hoa don thanh cong
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, int newStatus)
        {
            // Lấy đơn hàng theo orderId từ repository
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception($"Không tìm thấy đơn hàng có ID = {orderId}");
            }

            // Nếu cần, bạn có thể kiểm tra giá trị newStatus hợp lệ (ví dụ chỉ cho phép 0, 1, 2)
            if (newStatus != 0 && newStatus != 1 && newStatus != 2)
            {
                throw new ArgumentException("Trạng thái đơn hàng không hợp lệ");
            }

            // Cập nhật trạng thái thanh toán và thời gian chỉnh sửa
            order.Status = newStatus;
            order.LastEdited = DateTime.UtcNow;

            // Cập nhật đơn hàng qua repository và trả về kết quả
            return await _orderRepository.UpdateAsync(order);
        }

        

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new Exception($"Not found with ID = {order}");
            }

            order.Status = 0;
            order.LastEdited = DateTime.UtcNow;
            return await _orderRepository.UpdateAsync(order);
        }

    }
}
