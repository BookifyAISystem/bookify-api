using bookify_data.DTOs.BookContentVersionDTO;

namespace bookify_service.Interfaces
{
    public interface IBookContentVersionService
    {
        Task CreateAsync(CreateBookContentVersionDTO dto);
        Task<GetBookContentVersionDTO?> GetByIdAsync(int id);
        Task<List<GetBookContentVersionDTO>> GetAllVersionsByBookIdAsync(int bookId);
        Task UpdateAsync(UpdateBookContentVersionDTO dto);
        Task DeleteAsync(int id);
    }
}
