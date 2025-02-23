using AutoMapper;
using bookify_data.Entities;
using bookify_data.Interfaces;
using bookify_data.Model;
using bookify_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetOrderDetailDTO>> GetAllAsync()
        {
            var orderDetailList = await _orderDetailRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetOrderDetailDTO>>(orderDetailList);
        }

        public async Task<GetOrderDetailDTO?> GetByIdAsync(int id)
        {
            var orderDetail = await _orderDetailRepository.GetByIdAsync(id);
            return _mapper.Map<GetOrderDetailDTO>(orderDetail);
        }

        public async Task<bool> CreateOrderDetailAsync(AddOrderDetailDTO addOrderDetailDto)
        {
            var orderDetailToAdd = _mapper.Map<OrderDetail>(addOrderDetailDto);
            return await _orderDetailRepository.InsertAsync(orderDetailToAdd);
        }

        public async Task<bool> UpdateOrderDetailAsync(int id, UpdateOrderDetailDTO updateOrderDetailDto)
        {
            var orderDetail = await _orderDetailRepository.GetByIdAsync(id);
            if (orderDetail == null)
                return false;

            _mapper.Map(updateOrderDetailDto, orderDetail);
            orderDetail.LastEdited = DateTime.UtcNow;

            return await _orderDetailRepository.UpdateAsync(orderDetail);
        }

        public async Task<bool> DeleteOrderDetailAsync(int id)
        {
            return await _orderDetailRepository.DeleteAsync(id);
        }
    }
}
