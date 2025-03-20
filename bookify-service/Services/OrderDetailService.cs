using AutoMapper;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
using bookify_data.Repository;
using bookify_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Slapper.AutoMapper;

namespace bookify_service.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IMapper mapper, IOrderRepository orderRepository, IVoucherRepository voucherRepository)
        {
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
            _orderRepository = orderRepository;
            _voucherRepository = voucherRepository;
        }

        public async Task<IEnumerable<GetOrderDetailDTO>> GetAllOrderDetailAsync()
        {
            var orderDetailList = await _orderDetailRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetOrderDetailDTO>>(orderDetailList);
        }

        public async Task<GetOrderDetailDTO?> GetByOrderDetailByIdAsync(int id)
        {
            var orderDetail = await _orderDetailRepository.GetByIdAsync(id);
            return _mapper.Map<GetOrderDetailDTO>(orderDetail);
        }

        public async Task<bool> AddOrderDetailAsync(AddOrderDetailDTO addOrderDetailDto)
        {
                var order = await _orderRepository.GetByIdAsync(addOrderDetailDto.OrderId);
                if (order == null)
                {
                    throw new Exception($"Order not found with ID = {addOrderDetailDto.OrderId}");
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
                    Price = addOrderDetailDto.Price,
                    CreatedDate = DateTime.UtcNow,
                    LastEdited = DateTime.UtcNow,
                    Status = 1
                };

                order.OrderDetails.Add(orderDetailToAdd);

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
               return await _orderRepository.UpdateAsync(order);
               
        }



        public async Task<bool> UpdateOrderDetailAsync(int id, UpdateOrderDetailDTO updateOrderDetailDto)
        {
            var orderDetail = await _orderDetailRepository.GetByIdAsync(id);
            if (orderDetail == null)
                return false;
            if (updateOrderDetailDto.Status != 1 && updateOrderDetailDto.Status != 0)
            {
                throw new ArgumentException("Invalid OrderDetail");
            }
            _mapper.Map(updateOrderDetailDto, orderDetail);
            orderDetail.LastEdited = DateTime.UtcNow;

            return await _orderDetailRepository.UpdateAsync(orderDetail);
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

        public async Task<bool> DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _orderDetailRepository.GetByIdAsync(id);
            if (orderDetail == null)
            {
                throw new Exception($"Not found with ID = {orderDetail}");
            }

            orderDetail.Status = 0;
            orderDetail.LastEdited = DateTime.UtcNow;
            return await _orderDetailRepository.UpdateAsync(orderDetail);
        }

        private async Task<int> CalculateOrderTotal(int orderId)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null) return 0;
            return order.OrderDetails.Sum(x => x.Quantity * x.Price);
        }
    }
}
