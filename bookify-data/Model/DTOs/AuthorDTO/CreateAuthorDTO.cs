using System.ComponentModel.DataAnnotations;

namespace bookify_data.DTOs.AuthorDTO
{
    public class CreateAuthorDTO
    {
        [Required]
        public string AuthorName { get; set; }

        public string? Content { get; set; }
    }
}
