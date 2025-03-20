using bookify_data.Model;
using bookify_service.Interfaces;
using bookify_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController (ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategoryDTO>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCategoryDTO>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound(new { message = "Category not found" });

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] AddCategoryDTO addCategoryDTO)
        {
            bool isCreated = await _categoryService.CreateCategoryAsync(addCategoryDTO);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create category" });

            return StatusCode(201, new { message = "Category created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO updateCategoryDto)
        {
            bool isUpdated = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
            if (!isUpdated)
                return NotFound(new { message = "Category not found or update failed" });

            return NoContent(); // HTTP 204
        }

        [HttpPatch("change-status/{id}")]
        public async Task<IActionResult> UpdateCategoryStatus(int id, [FromBody] int status)
        {
            try
            {
                bool isUpdate = await _categoryService.UpdateCategoryStatusAsync(id, status);

                if (!isUpdate)
                {
                    return NotFound($"Not found or update failed");
                }
                return Ok("Update Successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                bool isDeleted = await _categoryService.DeleteCategoryAsync(id);
                if (!isDeleted)
                    return NotFound(new { message = "Not found or delete failed" });

                return Ok("Delete Success (Status = 0).");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
