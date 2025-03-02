using bookify_data.Entities;

namespace bookify_data.Repositories.Interfaces
{
    public interface IBookContentVersionRepository
    {
        Task AddAsync(BookContentVersion entity);
        Task<BookContentVersion?> GetByIdAsync(int id);
        Task<List<BookContentVersion>> GetAllVersionsByBookIdAsync(int bookId);
        Task UpdateAsync(BookContentVersion entity);
        Task DeleteAsync(int id);
        Task<BookContentVersion?> GetLatestVersionByBookIdAsync(int bookId);  
        Task SaveChangesAsync();
    }
}
