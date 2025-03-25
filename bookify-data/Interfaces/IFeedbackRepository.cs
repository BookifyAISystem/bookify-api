using bookify_data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<IEnumerable<Feedback>> GetAllAsync();
        Task<IEnumerable<Feedback>> GetByAccountIdAndStatusAsync(int accountId, int status);
        Task<IEnumerable<Feedback>> GetByBookId(int bookId);
        Task<Feedback?> GetByIdAsync(int id);
        Task<bool> InsertAsync(Feedback feedback);
        Task<bool> UpdateAsync(Feedback feedback);

    }
}
