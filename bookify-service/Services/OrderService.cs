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

        public async Task<bool> CreateOrderAsync(AddOrderDTO addOrderDto)
        {
            var order = new Order
            {
                AccountId = addOrderDto.AccountId,
                CreatedDate = DateTime.UtcNow,
                LastEdited = DateTime.UtcNow,
                Total = 0,
                Status = 1 // active
            };
            return await _orderRepository.InsertAsync(order);
        }

        public async Task<bool> AddOrderDetailAsync(AddOrderDetailDTO addOrderDetailDto)
        {
            var order = await _orderRepository.GetByIdAsync(addOrderDetailDto.OrderId);
            if (order == null)
            {
                throw new Exception($"Order not found with ID = {addOrderDetailDto.OrderId}");
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

            if (addOrderDetailDto.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than 0");
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
            order.Total = await CalculateOrderTotal(order.OrderId);
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
            order.Total = await CalculateOrderTotal(order.OrderId);
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
            order.Total = await CalculateOrderTotal(order.OrderId);
            return await _orderRepository.UpdateAsync(order);

        }

        public async Task<bool> ConfirmOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception($"Order not found with ID = {orderId}");
            }

            if (order.Status != 1) throw new Exception("Order must be in 'Cart' status to confirm");
            order.Total = await CalculateOrderTotalWithVoucher(orderId);
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

        //public async Task<bool> UpdateOrderAsync(int id, UpdateOrderDTO updateOrderDto)
        //{
        //    var order = await _orderRepository.GetByIdAsync(id);
        //    if (order == null)
        //    {
        //        throw new Exception($"Not found with ID = {order}");
        //    }
        //    if (updateOrderDto.Status != 1 && updateOrderDto.Status != 0 && updateOrderDto.Status != 2)
        //    {
        //        throw new ArgumentException("Invalid Order");
        //    }
        //    order.Status = updateOrderDto.Status;
        //    if (updateOrderDto.Status == 0) // giả sử 0 = hủy
        //    {
        //        order.CancelReason = updateOrderDto.CancelReason;
        //    }
        //    order.LastEdited = DateTime.UtcNow;
        //    return await _orderRepository.UpdateAsync(order);
        //    // Truong hop bang 2 - thanh cong . Co the tao 1 DB danh cho luu tru hoa don thanh cong
        //}

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



        public async Task<bool> DeleteOrderAsync(int id, DeleteOrderDTO deleteOrderDTO)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new Exception($"Not found with ID = {order}");
            }
            if (order.Status != 1)
            {
                throw new ArgumentException("Can't cancel order without status 1");
            }
            order.CancelReason = deleteOrderDTO.CancelReason;
            order.Status = 0;
            order.LastEdited = DateTime.UtcNow;

            foreach (var orderDetail in order.OrderDetails)
            {
                var orderdetail = new OrderDetail
                {
                    Status = 0
                };
            }
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<bool> ConfirmOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception($"Order not found with ID = {orderId}");
            }
          
            if (order.Status != 1) throw new Exception("Order must be in 'Cart' status to confirm");
            order.Total = await CalculateOrderTotalWithVoucher(orderId);
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

        private async Task<int> CalculateOrderTotal(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return 0;
            return order.OrderDetails.Sum(x => x.Quantity * x.Price);
        }

        private async Task<int> CalculateOrderTotalWithVoucher(int orderId)
        {

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception($"Order not found with ID = {orderId}");
            }
            int total = order.Total;
            total = await CalculateOrderTotal(orderId);
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
            return total;
        }
    

    }
}
