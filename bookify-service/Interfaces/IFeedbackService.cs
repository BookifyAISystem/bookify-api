using bookify_data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IFeedbackService
    {
        Task<IEnumerable<GetFeedbackDTO>> GetAllAsync();

        Task<IEnumerable<GetFeedbackDTO>> GetFeedbacksByBookIdAsync();
        Task<GetFeedbackDTO?> GetByIdAsync(int id);

        Task<bool> CreateFeedbackAsync(AddFeedbackDTO addFeedbackDto);
        Task<bool> CreateFeedbacksByOrderDetailByOrderAsync(int orderId);
        Task<bool> UpdateFeedbackAsync(int id, UpdateFeedbackDTO updateFeedbackDto);
        Task<bool> ConfirmFeedBack(int feedbackId, int star, string feedbackContent);
        Task<bool> UpdateFeedbackStatusAsync(int id, int newStatus);
        Task<bool> DeleteFeedbackAsync(int id);

    }

}
