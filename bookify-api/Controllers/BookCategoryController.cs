using bookify_data.Model;
using bookify_service.Interfaces;
using bookify_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCategoryController : Controller
    {
        private readonly IBookCategoryService _bookCategoryService;
        public BookCategoryController(IBookCategoryService bookCategoryService)
        {
            _bookCategoryService = bookCategoryService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetCategoryDTO>>> GetAllBookCategories()
        {
            var bookCategories = await _bookCategoryService.GetAllAsync();
            return Ok(bookCategories);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<GetCategoryDTO>> GetBookCategoryById(int id)
        {
            var voucher = await _bookCategoryService.GetByIdAsync(id);
            if (voucher == null)
                return NotFound(new { message = "Category not found" });

            return Ok(voucher);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateBookCategory([FromBody] AddBookCategoryDTO addBookCategoryDTO)
        {
            bool isCreated = await _bookCategoryService.CreateBookCategoryAsync(addBookCategoryDTO);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create category" });

            return StatusCode(201, new { message = "Category created successfully" });
        }

        [HttpPut("UpdateById/{id}")]
        public async Task<ActionResult> UpdateBookCategory(int id, [FromBody] UpdateBookCategoryDTO updateBookCategoryDto)
        {
            bool isUpdated = await _bookCategoryService.UpdateBookCategoryAsync(id, updateBookCategoryDto);
            if (!isUpdated)
                return NotFound(new { message = "Category not found or update failed" });

            return NoContent(); // HTTP 204
        }
    }
}
