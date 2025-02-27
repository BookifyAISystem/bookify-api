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
        Task<GetFeedbackDTO?> GetByIdAsync(int id);
        Task<bool> CreateFeedbackAsync(AddFeedbackDTO addFeedbackDto);
        Task<bool> UpdateFeedbackAsync(int id, UpdateFeedbackDTO updateFeedbackDto);
    }
}
