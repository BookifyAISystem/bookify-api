using bookify_data.DTOs.AuthorDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_service.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<GetAuthorDTO>> GetAllAuthorsAsync();
        Task<GetAuthorDTO?> GetAuthorByIdAsync(int authorId);
        Task AddAuthorAsync(CreateAuthorDTO authorDto);
        Task UpdateAuthorAsync(UpdateAuthorDTO authorDto);
        Task DeleteAuthorAsync(int authorId);
        Task UpdateStatusAsync(int authorId, int status);

    }
}
