using System.ComponentModel.DataAnnotations;

namespace bookify_data.DTOs.BookContentVersionDTO
{
    public class UpdateBookContentVersionDTO
    {
        [Required(ErrorMessage = "BookContentVersionId là bắt buộc.")]
        public int BookContentVersionId { get; set; }

        [Required(ErrorMessage = "Cần có đúng 5 bản tóm tắt.")]
        [MinLength(5, ErrorMessage = "Phải có đúng 5 bản tóm tắt.")]
        [MaxLength(5, ErrorMessage = "Phải có đúng 5 bản tóm tắt.")]
        public List<string> Summaries { get; set; } = new List<string>();

        [Range(0, 1, ErrorMessage = "Status phải là 0 hoặc 1.")]
        public int Status { get; set; } = 1;
    }
}
