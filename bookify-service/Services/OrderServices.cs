using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Repository;
using bookify_data.Model;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;

        public OrderServices(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<GetOrdersDTO>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllOrdersAsync();
        }

        public async Task<GetOrderDTO?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task AddOrderAsync(AddOrderDTO orderDto)
        {
            var order = new Order
            {
                OrderId = orderDto.OrderId,
                Total = orderDto.Total,
                CreateDate = orderDto.CreateDate,
                CancelReason = orderDto.CancelReason,
                CustomerId = orderDto.CustomerId,
                VoucherId = orderDto.VoucherId,
            };

            await _orderRepository.AddOrderAsync(order);
        }


        public async Task UpdateOrderAsync(UpdateOrderDTO orderDto)
        {
            
            var order = await _orderRepository.GetOrderEntityByIdAsync(orderDto.OrderId);
            if (order != null)
            {
                order.OrderId = orderDto.OrderId;
                order.Total = orderDto.Total;
                order.CreateDate = orderDto.CreateDate;
                order.CancelReason = orderDto.CancelReason;
                    

                await _orderRepository.UpdateOrderAsync(order);
            }
        }



        public async Task DeleteOrderAsync(int orderId)
        {
            await _orderRepository.DeleteOrderAsync(orderId);
        }

        
    }
}
