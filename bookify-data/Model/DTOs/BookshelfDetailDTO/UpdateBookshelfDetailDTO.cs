using System.ComponentModel.DataAnnotations;

namespace bookify_data.DTOs.BookshelfDetailDTO
{
    public class UpdateBookshelfDetailDTO
    {
        [Required(ErrorMessage = "BookshelfDetailId is required.")]
        public int BookshelfDetailId { get; set; }

        [Required(ErrorMessage = "BookshelfId is required.")]
        public int BookshelfId { get; set; }

        [Required(ErrorMessage = "BookId is required.")]
        public int BookId { get; set; }
    }
}
