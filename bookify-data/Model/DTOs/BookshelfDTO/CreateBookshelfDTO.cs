using System;
using System.ComponentModel.DataAnnotations;

namespace bookify_data.DTOs.BookshelfDTO
{
    public class CreateBookshelfDTO
    {
        [Required(ErrorMessage = "AccountId is required.")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Bookshelf name is required.")]
        [StringLength(100, ErrorMessage = "Bookshelf name must be under 100 characters.")]
        public string BookShelfName { get; set; } = string.Empty;
    }
}
