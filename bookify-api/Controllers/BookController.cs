using bookify_data.DTOs;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        /// <summary>
        /// Lấy danh sách tất cả sách.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }

        /// <summary>
        /// Lấy chi tiết sách theo ID.
        /// </summary>
        [HttpGet("{id}")]
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
        [HttpPost]
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
        [HttpPut("{id}")]
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


        [HttpDelete("{id}")]
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

    }
}
