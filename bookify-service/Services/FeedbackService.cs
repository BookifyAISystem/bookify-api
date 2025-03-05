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
        private readonly IMapper _mapper;

        public FeedbackService(IFeedbackRepository feedbackRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _feedbackRepository = feedbackRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
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
        public async Task<bool> CreateFeedbackAsync(AddFeedbackDTO addFeedbackDTO)
        {
            var feedbackToAdd = _mapper.Map<Feedback>(addFeedbackDTO);
            feedbackToAdd.CreatedDate = DateTime.UtcNow;
            feedbackToAdd.LastEdited = DateTime.UtcNow;
            feedbackToAdd.Status = 1;
            return await _feedbackRepository.InsertAsync(feedbackToAdd);
        }
        public async Task<bool> CreateFeedbackIfOrderedAsync(AddFeedbackDTO addFeedbackDTO)
        {
            // Sử dụng method của OrderRepository để kiểm tra
            bool hasCompletedOrderForBook = await _orderRepository.HasCompletedOrderForBookAsync(
                addFeedbackDTO.AccountId, addFeedbackDTO.BookId);

            if (!hasCompletedOrderForBook)
            {
                throw new InvalidOperationException("You must have a completed order for this book before submitting feedback.");
            }

            // Tạo feedback sau khi xác nhận điều kiện
            var feedbackToAdd = _mapper.Map<Feedback>(addFeedbackDTO);
            feedbackToAdd.CreatedDate = DateTime.UtcNow;
            feedbackToAdd.LastEdited = DateTime.UtcNow;
            feedbackToAdd.Status = 1;
            return await _feedbackRepository.InsertAsync(feedbackToAdd);
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
    }
}
