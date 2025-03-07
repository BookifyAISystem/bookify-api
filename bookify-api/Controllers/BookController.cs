using bookify_data.DTOs;
using bookify_service.Interfaces;
using bookify_service.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_api.Controllers
{
    [Route("api")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        /// <summary>
        /// Lấy danh sách tất cả sách.
        /// </summary>      
        [HttpGet("books")]
        public async Task<IActionResult> GetAllBooks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 12)
        {
            try
            {
                var (books, totalCount) = await _bookService.GetAllBooksAsync(pageNumber, pageSize);

                return Ok(new
                {
                    totalItems = totalCount,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    currentPage = pageNumber,
                    books
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }


        /// <summary>
        /// Lấy chi tiết sách theo ID.
        /// </summary>
        [HttpGet("book/{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var book = await _bookService.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound(new { message = "Book not found." });
                }
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Thêm mới sách với hình ảnh upload lên AWS S3.
        /// </summary>
        [HttpPost ("book")]
        public async Task<IActionResult> AddBook([FromForm] AddBookDTO bookDto)
        {
            try
            {
                await _bookService.AddBookAsync(bookDto);
                return Ok(new { message = "Book added successfully!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin sách, hỗ trợ thay đổi ảnh trên AWS S3.
        /// </summary>
        [HttpPut("book/{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] UpdateBookDTO bookDto)
        {
            if (id != bookDto.BookId)
            {
                return BadRequest(new { message = "Book ID mismatch." });
            }

            try
            {
                await _bookService.UpdateBookAsync(bookDto);
                return Ok(new { message = "Book updated successfully!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }


        [HttpDelete("book/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await _bookService.DeleteBookAsync(id);
                return Ok(new { message = "Book deleted successfully (soft delete)!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
        /// <summary>
        /// Gợi ý sách dựa trên ký tự nhập vào.
        /// </summary>
        [HttpGet("books/suggest")]
        public async Task<IActionResult> SuggestBooks([FromQuery] string query, [FromQuery] int limit = 5)
        {
            try
            {
                var books = await _bookService.SuggestBooksAsync(query, limit);
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while suggesting books.", details = ex.Message });
            }
        }

        /// <summary>
        /// Tìm kiếm sách có phân trang.
        /// </summary>
        [HttpGet("book/search")]
        public async Task<IActionResult> SearchBooks([FromQuery] string query, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 12)
        {
            try
            {
                var (books, totalPages) = await _bookService.SearchBooksAsync(query, pageNumber, pageSize);

                return Ok(new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    Books = books
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while searching books.", details = ex.Message });
            }
        }
        [HttpPatch("book/{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] int status)
        {
            await _bookService.UpdateStatusAsync(id, status);
            return Ok(new { message = "Book status updated successfully!" });
        }

        [HttpGet("books/latest")]
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
        [HttpGet("books/bestsellers")]
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
    }
}
