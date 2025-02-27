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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategoryDTO>>> GetAllBookCategories()
        {
            var bookCategories = await _bookCategoryService.GetAllAsync();
            return Ok(bookCategories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCategoryDTO>> GetBookCategoryById(int id)
        {
            var bookCategory = await _bookCategoryService.GetByIdAsync(id);
            if (bookCategory == null)
                return NotFound(new { message = "Book Category not found" });

            return Ok(bookCategory);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBookCategory([FromBody] AddBookCategoryDTO addBookCategoryDTO)
        {
            bool isCreated = await _bookCategoryService.CreateBookCategoryAsync(addBookCategoryDTO);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create book category" });

            return StatusCode(201, new { message = "Book Category created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBookCategory(int id, [FromBody] UpdateBookCategoryDTO updateBookCategoryDto)
        {
            bool isUpdated = await _bookCategoryService.UpdateBookCategoryAsync(id, updateBookCategoryDto);
            if (!isUpdated)
                return NotFound(new { message = "Book Category not found or update failed" });

            return NoContent(); // HTTP 204
        }
    }
}
