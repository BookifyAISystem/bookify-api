using System.ComponentModel.DataAnnotations;

namespace bookify_data.DTOs.BookContentVersionDTO
{
    public class CreateBookContentVersionDTO
    {
        [Required(ErrorMessage = "BookId là bắt buộc.")]
        [Range(1, int.MaxValue, ErrorMessage = "BookId phải lớn hơn 0.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Phải có đủ 5 bản tóm tắt.")]
        [MinLength(5, ErrorMessage = "Cần ít nhất 5 bản tóm tắt.")]
        public List<string> Summaries { get; set; } = new List<string>();

        [Range(1, int.MaxValue, ErrorMessage = "Version phải lớn hơn 0.")]
        public int Version { get; set; } = 1;

        [Range(0, 1, ErrorMessage = "Status phải là 0 hoặc 1.")]
        public int Status { get; set; } = 1;
    }
}
