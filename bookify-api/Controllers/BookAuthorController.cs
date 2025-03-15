using bookify_data.DTOs.BookAuthorDTO;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_api.Controllers
{
    [Route("api")]
    [ApiController]
    public class BookAuthorController : ControllerBase
    {
        private readonly IBookAuthorService _bookAuthorService;

        public BookAuthorController(IBookAuthorService bookAuthorService)
        {
            _bookAuthorService = bookAuthorService;
        }

        [HttpGet("bookauthors")]
        public async Task<IEnumerable<GetBookAuthorDTO>> GetAllBookAuthors()
        {
            return await _bookAuthorService.GetAllBookAuthorsAsync();
        }

        [HttpGet("bookauthors/{id}")]
        public async Task<ActionResult<GetBookAuthorDTO>> GetBookAuthorById(int id)
        {
            var bookAuthor = await _bookAuthorService.GetBookAuthorByIdAsync(id);
            if (bookAuthor == null) return NotFound(new { message = "BookAuthor not found." });
            return Ok(bookAuthor);
        }

        [HttpPost("bookauthors")]
        public async Task<IActionResult> AddBookAuthor([FromBody] CreateBookAuthorDTO bookAuthorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookAuthorService.AddBookAuthorAsync(bookAuthorDto);
            return Ok(new { message = "BookAuthor created successfully!" });
        }

        [HttpPut("bookauthors/{id}")]
        public async Task<IActionResult> UpdateBookAuthor(int id, [FromBody] UpdateBookAuthorDTO bookAuthorDto)
        {
            if (id != bookAuthorDto.BookAuthorId)
            {
                return BadRequest(new { message = "BookAuthor ID mismatch." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _bookAuthorService.UpdateBookAuthorAsync(bookAuthorDto);
            return Ok(new { message = "BookAuthor updated successfully!" });
        }

        [HttpDelete("bookauthors/{id}")]
        public async Task<IActionResult> DeleteBookAuthor(int id)
        {
            await _bookAuthorService.DeleteBookAuthorAsync(id);
            return Ok(new { message = "BookAuthor deleted successfully!" });
        }
        [HttpPatch("bookauthors{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] int status)
        {
            await _bookAuthorService.UpdateStatusAsync(id, status);
            return Ok(new { message = "BookAuthor status updated successfully!" });
        }

    }
}
