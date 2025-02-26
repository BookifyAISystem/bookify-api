using bookify_data.Model;
using bookify_service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetOrderDetailDTO>>> GetAllOrderDetails()
        {
            var orderDetails = await _orderDetailService.GetAllAsync();
            return Ok(orderDetails);
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<GetOrderDetailDTO>> GetOrderDetailById(int id)
        {
            var orderDetail = await _orderDetailService.GetByIdAsync(id);
            if (orderDetail == null)
                return NotFound(new { message = "Order detail not found" });

            return Ok(orderDetail);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateOrderDetail([FromBody] AddOrderDetailDTO addOrderDetailDto)
        {
            bool isCreated = await _orderDetailService.CreateOrderDetailAsync(addOrderDetailDto);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create order detail" });

            return StatusCode(201, new { message = "Order detail created successfully" });
        }

        [HttpPut("UpdateById/{id}")]
        public async Task<ActionResult> UpdateOrderDetail(int id, [FromBody] UpdateOrderDetailDTO updateOrderDetailDto)
        {
            bool isUpdated = await _orderDetailService.UpdateOrderDetailAsync(id, updateOrderDetailDto);
            if (!isUpdated)
                return NotFound(new { message = "Order detail not found or update failed" });

            return NoContent(); // HTTP 204
        }

        
    }
}
