using bookify_data.DTOs.AuthorDTO;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_api.Controllers
{
    [Route("api/v1/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Lấy danh sách tất cả tác giả (chỉ hiển thị những tác giả có `Status = 1`).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            return Ok(authors);
        }

        /// <summary>
        /// Lấy chi tiết một tác giả theo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            try
            {
                var author = await _authorService.GetAuthorByIdAsync(id);
                if (author == null)
                {
                    return NotFound(new { message = "Author not found." });
                }
                return Ok(author);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Thêm mới tác giả (hỗ trợ upload ảnh).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromForm] CreateAuthorDTO authorDto)
        {
            try
            {
                await _authorService.AddAuthorAsync(authorDto);
                return Ok(new { message = "Author added successfully!" });
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
        /// Cập nhật thông tin tác giả (hỗ trợ cập nhật ảnh).
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromForm] UpdateAuthorDTO authorDto)
        {
            if (id != authorDto.AuthorId)
            {
                return BadRequest(new { message = "Author ID mismatch." });
            }

            try
            {
                await _authorService.UpdateAuthorAsync(authorDto);
                return Ok(new { message = "Author updated successfully!" });
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

        /// <summary>
        /// Xóa tác giả bằng soft delete (cập nhật `Status = 0`).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            try
            {
                await _authorService.DeleteAuthorAsync(id);
                return Ok(new { message = "Author deleted successfully!" });
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
        /// Cập nhật trạng thái tác giả (0: Ẩn, 1: Hiển thị).
        /// </summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] int status)
        {
            await _authorService.UpdateStatusAsync(id, status);
            return Ok(new { message = "Author status updated successfully!" });
        }
    }
}
