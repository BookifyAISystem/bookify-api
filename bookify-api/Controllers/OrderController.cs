using bookify_data.Model;
using bookify_service.Interfaces;
using bookify_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderDTO>> GetOrderById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound(new { message = "Order not found" });

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] AddOrderDTO addOrderDto)
        {
            bool isCreated = await _orderService.CreateOrderAsync(addOrderDto);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create order" });

            return StatusCode(201, new { message = "Order created successfully" });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO updateOrderDto)
        {
            bool isUpdated = await _orderService.UpdateOrderAsync(id, updateOrderDto);
            if (!isUpdated)
                return NotFound(new { message = "Order not found or update failed" });

            return NoContent(); // HTTP 204
        }

        [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        bool isDeleted = await _orderService.DeleteOrderAsync(id);
        if (!isDeleted)
            return NotFound(new { message = "Order not found" });

        return NoContent(); // HTTP 204
    }
    }
}
