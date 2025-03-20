using System.ComponentModel.DataAnnotations;

namespace bookify_data.DTOs.BookAuthorDTO
{
    public class UpdateBookAuthorDTO
    {
        [Required(ErrorMessage = "BookAuthorId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "BookAuthorId must be greater than 0.")]
        public int BookAuthorId { get; set; }

        [Required(ErrorMessage = "BookId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "BookId must be greater than 0.")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "AuthorId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "AuthorId must be greater than 0.")]
        public int AuthorId { get; set; }
    }
}
