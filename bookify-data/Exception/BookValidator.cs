using bookify_api.Repositories;

namespace bookify_api.Validators
{
    public class BookValidator
    {
        private readonly IBookRepository _bookRepository;

        public BookValidator(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task ValidateAsync(object bookDto)
        {
            dynamic dto = bookDto;

            if (string.IsNullOrWhiteSpace(dto.BookName))
            {
                throw new ArgumentException("BookName is required.");
            }

            if (await IsDuplicateBookName(dto.BookName, dto.BookId))
            {
                throw new InvalidOperationException($"A book with the name '{dto.BookName}' already exists.");
            }

            if (dto.Price == null || dto.Price <= 0)
            {
                throw new ArgumentException("Price must be a positive number.");
            }

            if (dto.PulishYear <= 0)
            {
                throw new ArgumentException("Publish year must be greater than zero.");
            }

            if (dto.PulishYear > DateTime.Now.Year)
            {
                throw new InvalidOperationException("Publish year cannot be in the future.");
            }

            if (dto.AuthorId <= 0)
            {
                throw new ArgumentException("Invalid AuthorId.");
            }

            if (dto.CategoryId <= 0)
            {
                throw new ArgumentException("Invalid CategoryId.");
            }

            if (dto.CollectionId <= 0)
            {
                throw new ArgumentException("Invalid CollectionId.");
            }

            if (dto.PromotionId <= 0)
            {
                throw new ArgumentException("Invalid PromotionId.");
            }

            if (dto.BookType.Length > 255)
            {
                throw new ArgumentException("BookType must not exceed 255 characters.");
            }

            if (dto.Description.Length > 2000)
            {
                throw new ArgumentException("Description must not exceed 2000 characters.");
            }
        }

        private async Task<bool> IsDuplicateBookName(string bookName, int bookId)
        {
            var existingBook = await _bookRepository.GetBookByNameAsync(bookName);
            return existingBook != null && existingBook.BookId != bookId;
        }
    }
}
