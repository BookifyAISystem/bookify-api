using bookify_data.DTOs.WishlistDTO;
using bookify_data.DTOs.WishlistDetailDTO;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bookify_api.Controllers
{
    [Route("api/v1/wishlists")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        private readonly IWishlistDetailService _wishlistDetailService;

        public WishlistController(IWishlistService wishlistService, IWishlistDetailService wishlistDetailService)
        {
            _wishlistService = wishlistService;
            _wishlistDetailService = wishlistDetailService;
        }

        // Wishlist API
        [HttpGet]
        public async Task<IActionResult> GetAllWishlists()
        {
            var wishlists = await _wishlistService.GetAllWishlistsAsync();
            return Ok(wishlists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWishlistById(int id)
        {
            var wishlist = await _wishlistService.GetWishlistByIdAsync(id);
            return wishlist != null ? Ok(wishlist) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddWishlist([FromBody] CreateWishlistDTO dto)
        {
            await _wishlistService.AddWishlistAsync(dto);
            return Ok(new { message = "Wishlist created successfully!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWishlist(int id, [FromBody] UpdateWishlistDTO dto)
        {
            if (id != dto.WishlistId) return BadRequest("ID mismatch!");
            await _wishlistService.UpdateWishlistAsync(dto);
            return Ok(new { message = "Wishlist updated successfully!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            await _wishlistService.DeleteWishlistAsync(id);
            return Ok(new { message = "Wishlist deleted successfully!" });
        }

        // WishlistDetail API
        [HttpGet("details")]
        public async Task<IActionResult> GetAllWishlistDetails()
        {
            return Ok(await _wishlistDetailService.GetAllWishlistDetailsAsync());
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetWishlistDetailById(int id)
        {
            var result = await _wishlistDetailService.GetWishlistDetailByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost("details")]
        public async Task<IActionResult> AddWishlistDetail([FromBody] CreateWishlistDetailDTO dto)
        {
            await _wishlistDetailService.AddWishlistDetailAsync(dto);
            return Ok(new { message = "WishlistDetail created successfully!" });
        }

        [HttpPut("details")]
        public async Task<IActionResult> UpdateWishlistDetail([FromBody] UpdateWishlistDetailDTO dto)
        {
            await _wishlistDetailService.UpdateWishlistDetailAsync(dto);
            return Ok(new { message = "WishlistDetail updated successfully!" });
        }

        [HttpDelete("details/{id}")]
        public async Task<IActionResult> DeleteWishlistDetail(int id)
        {
            await _wishlistDetailService.DeleteWishlistDetailAsync(id);
            return Ok(new { message = "WishlistDetail deleted successfully!" });
        }
    }
}