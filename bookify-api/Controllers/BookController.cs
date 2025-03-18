using bookify_data.DTOs;
using bookify_data.DTOs.BookAuthorDTO;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_api.Controllers
{
    [Route("api/v1/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IBookAuthorService _bookAuthorService;

        public BookController(IBookService bookService, IBookAuthorService bookAuthorService)
        {
            _bookService = bookService;
            _bookAuthorService = bookAuthorService;
        }

        // --- API liên quan đến Book ---
        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] string query = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 12)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(query))
                {
                    var (books, totalPages) = await _bookService.SearchBooksAsync(query, pageNumber, pageSize);
                    return Ok(new { isSearch = true, query, pageNumber, pageSize, totalPages, books });
                }
                else
                {
                    var (books, totalCount) = await _bookService.GetAllBooksAsync(pageNumber, pageSize);
                    return Ok(new { isSearch = false, totalItems = totalCount, totalPages = (int)Math.Ceiling((double)totalCount / pageSize), currentPage = pageNumber, books });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return book != null ? Ok(book) : NotFound(new { message = "Book not found." });
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromForm] AddBookDTO bookDto)
        {
            await _bookService.AddBookAsync(bookDto);
            return Ok(new { message = "Book added successfully!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] UpdateBookDTO bookDto)
        {
            if (id != bookDto.BookId) return BadRequest(new { message = "Book ID mismatch." });
            await _bookService.UpdateBookAsync(bookDto);
            return Ok(new { message = "Book updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return Ok(new { message = "Book deleted successfully (soft delete)!" });
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] int status)
        {
            await _bookService.UpdateStatusAsync(id, status);
            return Ok(new { message = "Book status updated successfully!" });
        }

        [HttpGet("bestsellers")]
        public async Task<IActionResult> GetBestSellingBooks([FromQuery] int count = 8)
        {
            try
            {
                var books = await _bookService.GetBestSellingBooksAsync(count);
                return Ok(new { message = "Success", data = books });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestBooks([FromQuery] int count = 8)
        {
            try
            {
                var latestBooks = await _bookService.GetLatestBooksAsync(count);
                return Ok(latestBooks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving latest books.", details = ex.Message });
            }
        }

            // --- API liên quan đến BookAuthor ---
            [HttpGet("authors")]
        public async Task<IEnumerable<GetBookAuthorDTO>> GetAllBookAuthors()
        {
            return await _bookAuthorService.GetAllBookAuthorsAsync();
        }

        [HttpGet("authors/{id}")]
        public async Task<IActionResult> GetBookAuthorById(int id)
        {
            var bookAuthor = await _bookAuthorService.GetBookAuthorByIdAsync(id);
            return bookAuthor != null ? Ok(bookAuthor) : NotFound(new { message = "BookAuthor not found." });
        }

        [HttpPost("authors")]
        public async Task<IActionResult> AddBookAuthor([FromBody] CreateBookAuthorDTO bookAuthorDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _bookAuthorService.AddBookAuthorAsync(bookAuthorDto);
            return Ok(new { message = "BookAuthor created successfully!" });
        }

        [HttpPut("authors/{id}")]
        public async Task<IActionResult> UpdateBookAuthor(int id, [FromBody] UpdateBookAuthorDTO bookAuthorDto)
        {
            if (id != bookAuthorDto.BookAuthorId) return BadRequest(new { message = "BookAuthor ID mismatch." });
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _bookAuthorService.UpdateBookAuthorAsync(bookAuthorDto);
            return Ok(new { message = "BookAuthor updated successfully!" });
        }

        [HttpDelete("authors/{id}")]
        public async Task<IActionResult> DeleteBookAuthor(int id)
        {
            await _bookAuthorService.DeleteBookAuthorAsync(id);
            return Ok(new { message = "BookAuthor deleted successfully!" });
        }

        [HttpPatch("authors/{id}/status")]
        public async Task<IActionResult> UpdateBookAuthorStatus(int id, [FromQuery] int status)
        {
            await _bookAuthorService.UpdateStatusAsync(id, status);
            return Ok(new { message = "BookAuthor status updated successfully!" });
        }
    }
}