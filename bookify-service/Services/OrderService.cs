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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IVoucherRepository voucherRepository, IBookRepository bookRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _voucherRepository = voucherRepository;
            _bookRepository = bookRepository;
            _orderDetailRepository = orderDetailRepository;
            _unitOfWork = unitOfWork;
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
            if (newStatus != 0 && newStatus != 1 && newStatus != 2 && newStatus != 3 && newStatus != 4 )
            {
                throw new ArgumentException("Trạng thái đơn hàng không hợp lệ");
            }

            // Cập nhật trạng thái thanh toán và thời gian chỉnh sửa
            order.Status = newStatus;
            order.LastEdited = DateTime.UtcNow;
            _unitOfWork.Orders.Update(order);
            // Cập nhật đơn hàng qua repository và trả về kết quả
            return await _unitOfWork.CompleteAsync();
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
            _unitOfWork.OrderDetails.Update(orderDetail);
            return await _unitOfWork.CompleteAsync();
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
            _unitOfWork.Orders.Insert(order);
            return await _unitOfWork.CompleteAsync();
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
            _unitOfWork.Orders.Update(order);
            return await _unitOfWork.CompleteAsync();
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
            if (order == null)
            {
                throw new Exception($"Order not found with ID = {orderDetail.OrderId}");
            }
            if (orderDetail.Status != 1) throw new Exception("Order is not in 'Cart' status");

            var book = await _bookRepository.GetBookByIdAsync(orderDetail.BookId);
            if (book == null)
            {
                throw new Exception($"Book not found with ID = {orderDetail.BookId}");
            }
            order.Total = order.Total - orderDetail.Price;
            // Update
            orderDetail.Quantity = newQuantity;
            orderDetail.Price = book.Price * newQuantity;
            orderDetail.LastEdited = DateTime.UtcNow;

            order.Total = order.Total + orderDetail.Price;
            order.LastEdited = DateTime.UtcNow;

            _unitOfWork.Orders.Update(order);
            _unitOfWork.OrderDetails.Update(orderDetail);
             

            return await _unitOfWork.CompleteAsync();
        }
        public async Task<bool> RemoveOrderDetailAsync(int orderDetailId)
        {
            // 1. Lấy OrderDetail
            var orderDetail = await _orderDetailRepository.GetByIdAsync(orderDetailId);
            if (orderDetail == null)
                throw new Exception($"OrderDetail not found with ID = {orderDetailId}");

            // 2. Lấy Order
            var order = await _orderRepository.GetByIdAsync(orderDetail.OrderId);
            if (order == null)
                throw new Exception($"Order not found with ID = {orderDetail.OrderId}");

            // 3. Kiểm tra status ở Order thay vì OrderDetail
            if (order.Status != 1)
                throw new Exception("Order is not in 'Cart' status");


            // 5. Tính lại total
            order.Total = order.Total - orderDetail.Price;
            order.LastEdited = DateTime.UtcNow;
            // 6. Cập nhật Order
            _unitOfWork.Orders.Update(order);
            _unitOfWork.OrderDetails.Remove(orderDetail);


            return await _unitOfWork.CompleteAsync();
            

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
            _unitOfWork.Orders.Update(order);
            return await _unitOfWork.CompleteAsync();
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
            _unitOfWork.Orders.Update(order);
            return await _unitOfWork.CompleteAsync();
        }

        private  int CalculateOrderTotal(Order order)
        {
            
            return order.OrderDetails.Sum(x => x.Price);
        }


        public async Task<bool> UpdateOrderDetailsStatusAsync(int orderId, int newStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception($"Order not found with ID = {orderId}");
            }

            foreach (var orderDetail in order.OrderDetails)
            {
                orderDetail.Status = newStatus;
                orderDetail.LastEdited = DateTime.UtcNow;
                _unitOfWork.OrderDetails.Update(orderDetail);
            }
            _unitOfWork.Orders.Update(order);
            return await _unitOfWork.CompleteAsync();
        }



    }
}
