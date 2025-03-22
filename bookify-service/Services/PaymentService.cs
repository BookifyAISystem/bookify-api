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
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetPaymentDTO>> GetAllPaymentAsync()
        {
            var payments = await _unitOfWork.Payments.GetAllAsync();
            return _mapper.Map<IEnumerable<GetPaymentDTO>>(payments);
        }
        public async Task<IEnumerable<GetPaymentDTO>> GetPaymentsByAccountIdAsync(int accountId)
        {
            var payments = await _unitOfWork.Payments.GetPaymentsByAccountIdAsync(accountId);
            return _mapper.Map<IEnumerable<GetPaymentDTO>>(payments);
        }
        public async Task<IEnumerable<GetPaymentDTO>> GetPaymentsByStatusAsync(int status)
        {
            var payments = await _unitOfWork.Payments.GetPaymentsByStatus(status);
            return _mapper.Map<IEnumerable<GetPaymentDTO>>(payments);
        }
        public async Task<GetPaymentDTO?> GetPaymentByIdAsync(int id)
        {
            var payment = await _unitOfWork.Payments.GetByIdAsync(id);
            return _mapper.Map<GetPaymentDTO>(payment);
        }
        public async Task<bool> CreateVnpayPaymentAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            var payment = new Payment
            {
                Method = 1,
                PaymentDate = DateTime.Now,
                Amount = order.Total,
                OrderId = orderId,
                Status = 1,
                CreatedDate = DateTime.Now,
                LastEdited = DateTime.Now

            };
            _unitOfWork.Payments.Insert(payment);
            return await _unitOfWork.CompleteAsync();
            
        }
        public async Task<bool> CreateCODPaymentAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found");
            }
            var payment = new Payment
            {
                Method = 1,
                PaymentDate = DateTime.Now,
                Amount = order.Total,
                OrderId = orderId,
                Status = 1,
                CreatedDate = DateTime.Now,
                LastEdited = DateTime.Now

            };
            _unitOfWork.Payments.Insert(payment);
            return await _unitOfWork.CompleteAsync();

        }
    }
}
