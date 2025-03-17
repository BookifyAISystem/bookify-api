using bookify_data.DTOs.WishlistDetailDTO;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/v1/wishlist-details")]
    [ApiController]
    public class WishlistDetailController : ControllerBase
    {
        private readonly IWishlistDetailService _wishlistDetailService;

        public WishlistDetailController(IWishlistDetailService wishlistDetailService)
        {
            _wishlistDetailService = wishlistDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _wishlistDetailService.GetAllWishlistDetailsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _wishlistDetailService.GetWishlistDetailByIdAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateWishlistDetailDTO dto)
        {
            await _wishlistDetailService.AddWishlistDetailAsync(dto);
            return Ok(new { message = "WishlistDetail created successfully!" });

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateWishlistDetailDTO dto)
        {
            await _wishlistDetailService.UpdateWishlistDetailAsync(dto);
            return Ok(new { message = "WishlistDetail update successfully!" });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _wishlistDetailService.DeleteWishlistDetailAsync(id);
            return Ok(new { message = "WishlistDetail delete successfully!" });
        }
    }
}
