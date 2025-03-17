using bookify_data.DTOs.WishlistDTO;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/v1/wishlists")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

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
    }
}
