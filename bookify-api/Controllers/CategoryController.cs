using bookify_data.Model;
using bookify_service.Interfaces;
using bookify_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController (ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetCategoryDTO>>> GetAllVouchers()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<GetCategoryDTO>> GetCategoryById(int id)
        {
            var voucher = await _categoryService.GetByIdAsync(id);
            if (voucher == null)
                return NotFound(new { message = "Category not found" });

            return Ok(voucher);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateCategory([FromBody] AddCategoryDTO addCategoryDTO)
        {
            bool isCreated = await _categoryService.CreateVoucherAsync(addCategoryDTO);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create category" });

            return StatusCode(201, new { message = "Category created successfully" });
        }

        [HttpPut("UpdateById/{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO updateCategoryDto)
        {
            bool isUpdated = await _categoryService.UpdateVoucherAsync(id, updateCategoryDto);
            if (!isUpdated)
                return NotFound(new { message = "Category not found or update failed" });

            return NoContent(); // HTTP 204
        }
    }
}
