using Amazon.S3.Model;
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
        private readonly IBookRepository _bookRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IVoucherRepository voucherRepository, IBookRepository bookRepository, IOrderDetailRepository orderDetailRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _voucherRepository = voucherRepository;
            _bookRepository = bookRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<IEnumerable<GetOrderDTO>> GetAllOrderAsync()
        {
            var orderList = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetOrderDTO>>(orderList);

        }

        public async Task<IEnumerable<GetOrderDTO>> GetOrdersByStatusAsync(int status)
        {
            var orderList = await _orderRepository.GetOrdersByStatus(status);
            return _mapper.Map<IEnumerable<GetOrderDTO>>(orderList);

        }

        public async Task<IEnumerable<GetOrderDTO>> GetOrdersByAccountIdAsync(int accountId)
        {
            var orderList = await _orderRepository.GetOrdersByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<GetOrderDTO>>(orderList);
        }
        public async Task<GetOrderDTO?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return _mapper.Map<GetOrderDTO>(order);
        }

        public async Task<Order?> GetEntitesByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order;
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
        public async Task<IEnumerable<GetOrderDetailDTO>> GetAllOrderDetailAsync()
        {
            var orderDetailList = await _orderDetailRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetOrderDetailDTO>>(orderDetailList);
        }
        public async Task<GetOrderDetailDTO?> GetOrderDetailByIdAsync(int id)
        {
            var orderDetail = await _orderDetailRepository.GetByIdAsync(id);
            return _mapper.Map<GetOrderDetailDTO>(orderDetail);
        }
        public async Task<bool> UpdateOrderDetailStatusAsync(int id, int newStatus)
        {
            var orderDetail = await _orderDetailRepository.GetByIdAsync(id);
            if (orderDetail == null)
            {
                throw new Exception($"Not found with ID = {orderDetail}");
            }

            if (orderDetail.Status != 0 && orderDetail.Status != 1)
            {
                throw new ArgumentException("Invalid Status");
            }
            orderDetail.Status = newStatus;
            orderDetail.LastEdited = DateTime.UtcNow;
            return await _orderDetailRepository.UpdateAsync(orderDetail);
        }
        // Flow User
        public async Task<bool> CreateOrderAsync(int accountId)
        {
            var order = new Order
            {
                AccountId = accountId,
                CreatedDate = DateTime.UtcNow,
                LastEdited = DateTime.UtcNow,
                Total = 0,
                Status = 1 // active
            };
            return await _orderRepository.InsertAsync(order);
        }

        public async Task<bool> AddOrderDetailAsync(int orderId, AddOrderDetailDTO addOrderDetailDto)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception($"Order not found with ID = {orderId}");
            }
            var book = await _bookRepository.GetBookByIdAsync(addOrderDetailDto.BookId);
            if (book == null)
            {
                throw new Exception($"Book not found with ID = {addOrderDetailDto.BookId}");
            }
            if (addOrderDetailDto.Quantity < 1)
            {
                throw new ArgumentException("Quantity must be greater than or equal to 1");
            }

            
            var orderDetailToAdd = new OrderDetail
            {
                BookId = addOrderDetailDto.BookId,
                Quantity = addOrderDetailDto.Quantity,
                Price = book.Price * addOrderDetailDto.Quantity,
                CreatedDate = DateTime.UtcNow,
                LastEdited = DateTime.UtcNow,
                Status = 1
            };
            order.OrderDetails.Add(orderDetailToAdd);
            order.Total =  CalculateOrderTotal(order);
            return await _orderRepository.UpdateAsync(order);
        }
        public async Task<bool> UpdateOrderDetailQuantityAsync(int orderDetailId, int newQuantity)
        {
            var orderDetail = await _orderDetailRepository.GetByIdAsync(orderDetailId);
            if (orderDetail == null)
            {
                throw new Exception($"Order not found with ID = {orderDetailId}");
            }
            if (newQuantity < 1)
            {
                throw new ArgumentException("Quantity must be greater than or equal to 1");
            }

            var order = await _orderRepository.GetByIdAsync(orderDetail.OrderId);
            if (orderDetail.Status != 1) throw new Exception("Order is not in 'Cart' status");

            // Update
            orderDetail.Quantity = newQuantity;
            orderDetail.Price = orderDetail.Book.Price * newQuantity;
            orderDetail.LastEdited = DateTime.UtcNow;
            var updatedDetail = await _orderDetailRepository.UpdateAsync(orderDetail);

            // Recalculate total
            order.Total = CalculateOrderTotal(order);
            var updatedOrder = await _orderRepository.UpdateAsync(order);

            return updatedDetail && updatedOrder;
        }
        public async Task<bool> RemoveOrderDetailAsync(int orderDetailId)
        {
            var orderDetail = await _orderDetailRepository.GetByIdAsync(orderDetailId);
            if (orderDetail == null)
            {
                throw new Exception($"Order not found with ID = {orderDetailId}");
            }

            var order = await _orderRepository.GetByIdAsync(orderDetail.OrderId);
            if (orderDetail.Status != 1) throw new Exception("Order is not in 'Cart' status");

            // Delete
            var deleted = await _orderDetailRepository.DeleteAsync(orderDetailId);
            if (!deleted)
            {
                throw new Exception("Delete Failed");
            }
            // Recalculate total
            order.Total =  CalculateOrderTotal(order);
            return await _orderRepository.UpdateAsync(order);

        }

        public async Task<bool> ConfirmOrderAsync(int orderId, int voucherId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception($"Order not found with ID = {orderId}");
            }
            if (voucherId != 0)
            {

                var voucher = await _voucherRepository.GetByIdAsync(voucherId);
                if (voucher == null)
                {
                    throw new Exception($"Voucher not found with ID = {voucherId}");
                }
                order.VoucherId = voucherId;
            }
            if (order.Status != 1) throw new Exception("Order must be in 'Cart' status to confirm");
            
            if (order.VoucherId.HasValue)
            {
                var voucher = await _voucherRepository.GetByIdAsync(order.VoucherId.Value);
                if (voucher != null)
                {
                    if (order.Total >= voucher.MinAmount && voucher.Quantity > 0)
                    {
                        int discountValue = (int)(order.Total * (voucher.Discount / 100.0));
                        if (discountValue > voucher.MaxDiscount)
                        {
                            discountValue = voucher.MaxDiscount;
                        }
                        order.Total -= discountValue;
                        voucher.Quantity--;
                        voucher.LastEdited = DateTime.UtcNow;
                        await _voucherRepository.UpdateAsync(voucher);

                    }
                }
            }
           
            order.Status = 2;
            order.LastEdited = DateTime.UtcNow;
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> CancelOrderAsync(int orderId, string cancelReason)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception($"Order not found with ID = {orderId}");
            }

            if (order.Status != 2) throw new Exception("Order must be in 'Process' status to cancel");

            order.Status = 4;
            order.CancelReason = cancelReason;
            order.LastEdited = DateTime.UtcNow;
            return await _orderRepository.UpdateAsync(order);
        }

        private  int CalculateOrderTotal(Order order)
        {
            
            return order.OrderDetails.Sum(x => x.Price);
        }

        
    

    }
}
