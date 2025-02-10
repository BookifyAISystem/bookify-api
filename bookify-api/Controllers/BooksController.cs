using bookify_api.DTOs.BookDTO;
using bookify_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace bookify_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetBooksDTO>>> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetBookDTO>> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult> AddBook([FromForm] AddBookDTO bookDto, IFormFile? imageFile)
        {
            await _bookService.AddBookAsync(bookDto, imageFile);
            return CreatedAtAction(nameof(GetBookById), new { id = bookDto }, bookDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBook(int id, [FromForm] UpdateBookDTO bookDto, IFormFile? imageFile)
        {
            if (id != bookDto.BookId)
            {
                return BadRequest();
            }
            await _bookService.UpdateBookAsync(bookDto, imageFile);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}
