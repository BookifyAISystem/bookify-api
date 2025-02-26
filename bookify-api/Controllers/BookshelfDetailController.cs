using bookify_data.DTOs.BookshelfDetailDTO;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace bookify_api.Controllers
{
    [Route("api/book-detail")]
    [ApiController]
    public class BookshelfDetailController : ControllerBase
    {
        private readonly IBookshelfDetailService _bookshelfDetailService;

        public BookshelfDetailController(IBookshelfDetailService bookshelfDetailService)
        {
            _bookshelfDetailService = bookshelfDetailService;
        }

        /// <summary>
        /// Lấy danh sách tất cả BookshelfDetails
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllBookshelfDetails()
        {
            var bookshelfDetails = await _bookshelfDetailService.GetAllBookshelfDetailsAsync();
            return Ok(bookshelfDetails);
        }

        /// <summary>
        /// Lấy thông tin BookshelfDetail theo ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookshelfDetailById(int id)
        {
            var bookshelfDetail = await _bookshelfDetailService.GetBookshelfDetailByIdAsync(id);
            if (bookshelfDetail == null)
            {
                return NotFound(new { message = "BookshelfDetail not found." });
            }
            return Ok(bookshelfDetail);
        }

        /// <summary>
        /// Thêm mới một BookshelfDetail
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddBookshelfDetail([FromBody] AddBookshelfDetailDTO dto)
        {
            try
            {
                await _bookshelfDetailService.AddBookshelfDetailAsync(dto);
                return Ok(new { message = "BookshelfDetail added successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }

        /// <summary>
        /// Cập nhật thông tin BookshelfDetail
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookshelfDetail(int id, [FromBody] UpdateBookshelfDetailDTO dto)
        {
            if (id != dto.BookshelfDetailId)
            {
                return BadRequest(new { message = "BookshelfDetail ID mismatch." });
            }

            try
            {
                await _bookshelfDetailService.UpdateBookshelfDetailAsync(dto);
                return Ok(new { message = "BookshelfDetail updated successfully!" });
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
        /// Xóa một BookshelfDetail (Soft Delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookshelfDetail(int id)
        {
            try
            {
                await _bookshelfDetailService.DeleteBookshelfDetailAsync(id);
                return Ok(new { message = "BookshelfDetail deleted successfully (soft delete)!" });
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
