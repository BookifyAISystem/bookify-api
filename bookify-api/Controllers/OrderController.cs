using bookify_data.Model;
using bookify_service.Interfaces;
using bookify_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/v1/orders")]
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

        
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetOrdersByAccountId( int accountId)
        {
            var orders = await _orderService.GetOrdersByAccountIdAsync(accountId);
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

        [HttpPatch("change-status/{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] int status)
        {
            try
            {
                bool isUpdate = await _orderService.UpdateOrderStatusAsync(id, status);

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
        public async Task<ActionResult> DeleteOrder(int id)
        {
            try
            {
                bool isDeleted = await _orderService.DeleteOrderAsync(id);
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
