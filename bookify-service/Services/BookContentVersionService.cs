using bookify_data.DTOs.BookContentVersionDTO;
using bookify_data.Entities;
using bookify_data.Repositories.Interfaces;
using bookify_service.Interfaces;

namespace bookify_service.Services
{
    public class BookContentVersionService : IBookContentVersionService
    {
        private readonly IBookContentVersionRepository _repository;

        public BookContentVersionService(IBookContentVersionRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateAsync(CreateBookContentVersionDTO dto)
        {
            if (dto.Summaries.Count != 5)
                throw new ArgumentException("Cần đúng 5 bản tóm tắt.");

            var entity = new BookContentVersion
            {
                BookId = dto.BookId,
                Summary1 = dto.Summaries[0],
                Summary2 = dto.Summaries[1],
                Summary3 = dto.Summaries[2],
                Summary4 = dto.Summaries[3],
                Summary5 = dto.Summaries[4],
                Version = dto.Version,
                Status = dto.Status,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                LastEdited = DateTime.UtcNow.AddHours(7),
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task<GetBookContentVersionDTO?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return new GetBookContentVersionDTO
            {
                BookContentVersionId = entity.BookContentVersionId,
                BookId = entity.BookId,
                Summaries = new List<string>
                {
                    entity.Summary1 ?? "",
                    entity.Summary2 ?? "",
                    entity.Summary3 ?? "",
                    entity.Summary4 ?? "",
                    entity.Summary5 ?? ""
                },
                Version = entity.Version,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                LastEdited = DateTime.UtcNow.AddHours(7),
                Status = entity.Status
            };
        }

        public async Task<List<GetBookContentVersionDTO>> GetAllVersionsByBookIdAsync(int bookId)
        {
            var entities = await _repository.GetAllVersionsByBookIdAsync(bookId);
            return entities.Select(entity => new GetBookContentVersionDTO
            {
                BookContentVersionId = entity.BookContentVersionId,
                BookId = entity.BookId,
                Summaries = new List<string>
                {
                    entity.Summary1 ?? "",
                    entity.Summary2 ?? "",
                    entity.Summary3 ?? "",
                    entity.Summary4 ?? "",
                    entity.Summary5 ?? ""
                },
                Version = entity.Version,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                LastEdited = DateTime.UtcNow.AddHours(7),
                Status = entity.Status
            }).ToList();
        }

        public async Task UpdateAsync(UpdateBookContentVersionDTO dto)
        {
            var entity = await _repository.GetByIdAsync(dto.BookContentVersionId);
            if (entity == null)
                throw new KeyNotFoundException("Không tìm thấy phiên bản này.");

            entity.Summary1 = dto.Summaries[0];
            entity.Summary2 = dto.Summaries[1];
            entity.Summary3 = dto.Summaries[2];
            entity.Summary4 = dto.Summaries[3];
            entity.Summary5 = dto.Summaries[4];
            entity.Status = dto.Status;
            entity.LastEdited = DateTime.UtcNow.AddHours(7);

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
