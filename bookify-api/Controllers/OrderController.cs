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
        private readonly IOrderDetailService _orderDetailService;
        public OrderController(IOrderService orderService, IOrderDetailService orderDetailService)
        {
            _orderService = orderService;
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrderAsync();
            return Ok(orders);
        }

        
        [HttpGet("account/{accountId}")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetOrdersByAccountId( int accountId)
        {
            var orders = await _orderService.GetOrdersByAccountIdAsync(accountId);
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<GetOrderDTO>> GetOrderById(int orderId)
        {
            var order = await _orderService.GetByOrderDetailByIdAsync(orderId);
            if (order == null)
                return NotFound(new { message = "Order not found" });

            return Ok(order);
        }

        [HttpPost("{accountId}")]
        public async Task<ActionResult> CreateOrder(int accountId)
        {
            bool isCreated = await _orderService.CreateOrderAsync(accountId);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create order" });

            return StatusCode(201, new { message = "Order created successfully" });
        }

        [HttpPost("order-details")]
        public async Task<ActionResult> AddOrderDetailToOrder([FromBody] AddOrderDetailDTO addOrderDetailDto)
        {
            bool isCreated = await _orderDetailService.AddOrderDetailAsync(addOrderDetailDto);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create order detail" });

            return StatusCode(201, new { message = "Order detail created successfully" });
        }

        [HttpPut("order-details/{orderDetailId}")]
        public async Task<ActionResult> UpdateOrderDetailQuantity(int orderDetailId, [FromBody] int newQuantity)
        {
            bool isUpdated = await _orderDetailService.UpdateOrderDetailQuantityAsync(orderDetailId, newQuantity);
            if (!isUpdated)
                return NotFound(new { message = "Order detail not found or update failed" });

            return NoContent(); // HTTP 204
        }
        //[HttpPut("{id}")]
        //public async Task<ActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO updateOrderDto)
        //{
        //    try
        //    {
        //        bool isUpdated = await _orderService.UpdateOrderAsync(id, updateOrderDto);
        //        if (!isUpdated)
        //            return NotFound(new { message = "Order not found or update failed" });

        //        return NoContent(); // HTTP 204
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

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
        [HttpPost("confirm/{orderId}")]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            try
            {
                var result = await _orderService.ConfirmOrderAsync(orderId);
                if (!result) return BadRequest("Cannot confirm order");
                return Ok("Order confirmed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id, [FromBody] DeleteOrderDTO deleteOrderDTO)
        {
            try
            {
                bool isDeleted = await _orderService.DeleteOrderAsync(id, deleteOrderDTO);
                if (!isDeleted)
                    return NotFound(new { message = "Not found or delete failed" });

                return Ok("Delete Success (Status = 0).");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("order-details")]
        public async Task<ActionResult<IEnumerable<GetOrderDetailDTO>>> GetAllOrderDetails()
        {
            var orderDetails = await _orderDetailService.GetAllOrderDetailAsync();
            return Ok(orderDetails);
        }

        [HttpGet("order-details/{orderDetailId}")]
        public async Task<ActionResult<GetOrderDetailDTO>> GetOrderDetailById(int orderDetailId)
        {
            var orderDetail = await _orderDetailService.GetByOrderDetailByIdAsync(orderDetailId);
            if (orderDetail == null)
                return NotFound(new { message = "Order detail not found" });

            return Ok(orderDetail);
        }

        //[HttpPost("order-details")]
        //public async Task<ActionResult> CreateOrderDetail([FromBody] AddOrderDetailDTO addOrderDetailDto)
        //{
        //    bool isCreated = await _orderDetailService.AddOrderDetailAsync(addOrderDetailDto);
        //    if (!isCreated)
        //        return BadRequest(new { message = "Failed to create order detail" });

        //    return StatusCode(201, new { message = "Order detail created successfully" });
        //}

        //[HttpPut("order-details/{orderDetailId}")]
        //public async Task<ActionResult> UpdateOrderDetail(int orderDetailId, [FromBody] UpdateOrderDetailDTO updateOrderDetailDto)
        //{
        //    bool isUpdated = await _orderDetailService.UpdateOrderDetailAsync(orderDetailId, updateOrderDetailDto);
        //    if (!isUpdated)
        //        return NotFound(new { message = "Order detail not found or update failed" });

        //    return NoContent(); // HTTP 204
        //}

        [HttpPatch("order-details/change-status/{orderDetailId}")]
        public async Task<IActionResult> UpdateOrderDetailStatus(int orderDetailId, [FromBody] int status)
        {
            try
            {
                bool isUpdate = await _orderDetailService.UpdateOrderDetailStatusAsync(orderDetailId, status);

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
        [HttpDelete("order-details/{orderDetailId}")]
        public async Task<ActionResult> DeleteOrderDetail(int orderDetailId)
        {
            try
            {
                bool isDeleted = await _orderDetailService.DeleteOrderDetailAsync(orderDetailId);
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
