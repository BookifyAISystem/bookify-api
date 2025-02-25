using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace bookify_data.DTOs
{
    public class AddBookDTO
    {
        [Required(ErrorMessage = "Book name is required.")]
        public string BookName { get; set; }

        public string? BookType { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public int Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Ebook price must be greater than 0.")]
        public int PriceEbook { get; set; }

        public string? Description { get; set; }
        public string? BookContent { get; set; }

        [Range(1900, 2025, ErrorMessage = "Publish Year must be between 1900 and 2025.")]
        public int PublishYear { get; set; }

        public int? CategoryId { get; set; }

        public int? PromotionId { get; set; }
        public int? ParentBookId { get; set; }

        public int? AuthorId { get; set; }

        public IFormFile? ImageFile { get; set; } // Dùng để upload ảnh lên AWS S3
    }
}
