using bookify_data.DTOs.BookContentVersionDTO;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api")]
    [ApiController]
    public class BookContentVersionController : ControllerBase
    {
        private readonly IBookContentVersionService _service;

        public BookContentVersionController(IBookContentVersionService service)
        {
            _service = service;
        }

        [HttpPost("book-content-version")]
        public async Task<IActionResult> Create([FromBody] CreateBookContentVersionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CreateAsync(dto);
            return Ok(new { message = "BookContentVersion created successfully!" });
        }

        [HttpGet("book-content-version/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Không tìm thấy phiên bản này." });

            return Ok(result);
        }

        [HttpGet("book-content-versions/{bookId}")]
        public async Task<IActionResult> GetAllByBookId(int bookId)
        {
            var results = await _service.GetAllVersionsByBookIdAsync(bookId);
            return Ok(results);
        }

        [HttpPut("book-content-version")]
        public async Task<IActionResult> Update([FromBody] UpdateBookContentVersionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(dto);
            return Ok(new { message = "BookContentVersion updated successfully!" });
        }

        [HttpDelete("book-content-version/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = "BookContentVersion deleted successfully!" });
        }
    }
}
