using bookify_api.DTOs.BookDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using bookify_service.Interfaces;
using bookify_api.Validators;

namespace bookify_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly BookValidator _bookValidator;

        public BookController(IBookService bookService, BookValidator bookValidator)
        {
            _bookService = bookService;
            _bookValidator = bookValidator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetBooksDTO>>> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetBookDTO>> GetBookById(int? id)
        {
            if (!id.HasValue || id <= 0)
            {
                return BadRequest(new { message = "Invalid or missing book ID." });
            }

            try
            {
                var book = await _bookService.GetBookByIdAsync(id.Value);
                if (book == null)
                {
                    return NotFound(new { message = "Book not found." });
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddBook([FromForm] AddBookDTO bookDto, IFormFile? imageFile)
        {
            try
            {
                // Validate data using BookValidator
                await _bookValidator.ValidateAsync(bookDto);

                // Call service to add book
                await _bookService.AddBookAsync(bookDto, imageFile);
                return CreatedAtAction(nameof(GetBookById), new { id = bookDto }, bookDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, [FromForm] UpdateBookDTO bookDto, IFormFile? imageFile)
        {
            if (id != bookDto.BookId)
            {
                return BadRequest(new { message = "Book ID mismatch." });
            }

            try
            {
                // Validate data using BookValidator
                await _bookValidator.ValidateAsync(bookDto);

                // Call service to update book
                await _bookService.UpdateBookAsync(bookDto, imageFile);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}
