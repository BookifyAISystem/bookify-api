using bookify_data.Model;
using bookify_service.Interfaces;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

         public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        [HttpGet("getall")]

        public async Task<ActionResult<IEnumerable<GetOrdersDTO>>> GetAllOrders()
        {
            var orders = await _orderServices.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("getbyid")]
        public async Task<ActionResult<GetOrderDTO>> GetOrderById(int id)
        {
            var order = await _orderServices.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddOrder([FromForm] AddOrderDTO orderDto)
        {
            await _orderServices.AddOrderAsync(orderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderDto }, orderDto);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateOrder(int orderId, [FromForm] UpdateOrderDTO orderDto)
        {
            if (orderId != orderDto.OrderId)
            {
                return BadRequest();
            }
            await _orderServices.UpdateOrderAsync(orderDto);
            return NoContent();
        }

        [HttpDelete("deletebyid")]
        public async Task<ActionResult> DeleteOrder(int orderId)
        {
            await _orderServices.DeleteOrderAsync(orderId);
            return NoContent();
        }
    }
}
