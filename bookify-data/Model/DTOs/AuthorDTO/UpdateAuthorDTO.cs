using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace bookify_data.DTOs.AuthorDTO
{
    public class UpdateAuthorDTO
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public string AuthorName { get; set; }

        public string? Content { get; set; }
        public int Status { get; set; }
        public IFormFile? AuthorImageFile { get; set; }  // ✅ Hỗ trợ cập nhật ảnh mới

    }
}
