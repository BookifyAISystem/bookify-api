using AutoMapper;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
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
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
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
        public async Task<bool> CreateOrderAsync(AddOrderDTO addOrderDto)
        {
            var orderToAdd = _mapper.Map<Order>(addOrderDto); 
            return await _orderRepository.InsertAsync(orderToAdd); 

        }
        public async Task<bool> UpdateOrderAsync(int id, UpdateOrderDTO updateOrderDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return false;

            _mapper.Map(updateOrderDto, order);
            order.LastEdited = DateTime.UtcNow;

            return await _orderRepository.UpdateAsync(order); 
        }
        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }
    }
}
