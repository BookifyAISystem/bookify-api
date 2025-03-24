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
    public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        

        public FeedbackService(IFeedbackRepository feedbackRepository, IOrderRepository orderRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            
            _feedbackRepository = feedbackRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetFeedbackDTO>> GetAllAsync()
        {
            var feedbackList = await _feedbackRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GetFeedbackDTO>>(feedbackList);
        }
        public async Task<GetFeedbackDTO?> GetByIdAsync(int id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            return _mapper.Map<GetFeedbackDTO>(feedback);
        }

        public async Task<IEnumerable<GetFeedbackDTO>> GetByAccountIdAndStatusAsync(int accountId, int status)
        {
            var feedbackList = await _feedbackRepository.GetByAccountIdAndStatusAsync(accountId, status);
            return _mapper.Map<IEnumerable<GetFeedbackDTO>>(feedbackList);
        }
        public async Task<bool> CreateFeedbackAsync(AddFeedbackDTO addFeedbackDTO)
        {
            var feedbackToAdd = _mapper.Map<Feedback>(addFeedbackDTO);
            feedbackToAdd.CreatedDate = DateTime.UtcNow;
            feedbackToAdd.LastEdited = DateTime.UtcNow;
            feedbackToAdd.Status = 1;
            return await _feedbackRepository.InsertAsync(feedbackToAdd);
        }

        public async Task<bool> CreateFeedbacksByOrderDetailByOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }
            foreach (var orderDetail in order.OrderDetails)
            {
                var feedback = new Feedback
                {
                    Star = 0,
                    FeedbackContent = null,
                    AccountId = order.AccountId,
                    BookId = orderDetail.BookId,
                    CreatedDate = DateTime.UtcNow,
                    LastEdited = DateTime.UtcNow,
                    Status = 1,
                };
                await _feedbackRepository.InsertAsync(feedback);
            }

            
            return await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> ConfirmFeedBack(int feedbackId , int star, string feedbackContent)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(feedbackId);
            if (feedback == null)
            {
                throw new InvalidOperationException("Feedback not found");
            }
            feedback.Star = star;
            feedback.FeedbackContent = feedbackContent;
            feedback.LastEdited = DateTime.UtcNow;
            feedback.Status = 2;
            return await _feedbackRepository.UpdateAsync(feedback);
        }
        
        public async Task<bool> UpdateFeedbackAsync(int id, UpdateFeedbackDTO updateFeedbackDTO)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
                return false;
            if (updateFeedbackDTO.Status != 1 && updateFeedbackDTO.Status != 0)
            {
                throw new ArgumentException("Invalid feedback");
            }
            _mapper.Map(updateFeedbackDTO, feedback);
            feedback.CreatedDate = DateTime.UtcNow;
            return await _feedbackRepository.UpdateAsync(feedback);

        }

        public async Task<bool> UpdateFeedbackStatusAsync(int id, int newStatus)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
            {
                throw new Exception($"Not found with ID = {feedback}");
            }

            if (feedback.Status != 0 && feedback.Status != 1 )
            {
                throw new ArgumentException("Invalid Status");
            }
            feedback.Status = newStatus;
            feedback.LastEdited = DateTime.UtcNow;
            return await _feedbackRepository.UpdateAsync(feedback);
        }

        public async Task<bool> DeleteFeedbackAsync(int id)
        {
            var feedback = await _feedbackRepository.GetByIdAsync(id);
            if (feedback == null)
            {
                throw new Exception($"Not found with ID = {feedback}");
            }

            feedback.Status = 0;
            feedback.LastEdited = DateTime.UtcNow;
            return await _feedbackRepository.UpdateAsync(feedback);
        }
    }
}
