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
            var orders = await _orderService.GetAllOrderAsync();
            return Ok(orders);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetAllOrdersByStatus(int status)
        {
            var orders = await _orderService.GetOrdersByStatusAsync(status);
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
            var order = await _orderService.GetOrderByIdAsync(orderId);
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

        
        [HttpPost("{accountId}")]
        public async Task<ActionResult> CreateEmptyOrder(int accountId)
        {
            bool isCreated = await _orderService.CreateEmptyOrderByAccountIdAsync(accountId);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create order" });

            return StatusCode(201, new { message = "Order created successfully" });
        }

        [HttpPost("{orderId}/order-details")]
        public async Task<ActionResult> AddOrderDetailToOrder(int orderId, [FromBody] AddOrderDetailDTO addOrderDetailDto)
        {
            bool isCreated = await _orderService.AddOrderDetailAsync(orderId, addOrderDetailDto);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create order detail" });

            return StatusCode(201, new { message = "Order detail created successfully" });
        }

        //[HttpPut("order-details/{orderDetailId}")]
        //public async Task<ActionResult> UpdateOrderDetailQuantity(int orderDetailId, [FromBody] int newQuantity)
        //{
        //    bool isUpdated = await _orderService.UpdateOrderDetailQuantityAsync(orderDetailId, newQuantity);
        //    if (!isUpdated)
        //        return NotFound(new { message = "Order detail not found or update failed" });

        //    return NoContent(); // HTTP 204
        //}

        [HttpPut("order-details/{orderDetailId}")]
        public async Task<ActionResult> UpdateOrderDetailQuantity(int orderDetailId, [FromBody] UpdateOrderDetailDTO updateOrderDetailDTO)
        {
            bool isUpdated = await _orderService.UpdateOrderDetailAsync(orderDetailId, updateOrderDetailDTO);
            if (!isUpdated)
                return NotFound(new { message = "Order detail not found or update failed" });

            return NoContent(); // HTTP 204
        }


        [HttpPut("{orderId}")]
        public async Task<ActionResult> UpdateOrder(int orderId, [FromBody] UpdateOrderDTO updateOrderDto)
        {
            bool isUpdated = await _orderService.UpdateOrderAsync(orderId, updateOrderDto);
            if (!isUpdated)
                return NotFound(new { message = "Order not found or update failed" });

            return NoContent(); // HTTP 204
        }
        [HttpDelete("order-details/{orderDetailId}")]
        public async Task<ActionResult> RemoveOrderDetail(int orderDetailId)
        {
            bool isDeleted = await _orderService.RemoveOrderDetailAsync(orderDetailId);
            if (!isDeleted)
                return NotFound(new { message = "Order detail not found or delete failed" });

            return NoContent(); // HTTP 204
        }
        [HttpPatch("{orderId}/confirm")]
        public async Task<IActionResult> ConfirmOrder(int orderId, int voucherId)
        {
            try
            {
                var confirmed = await _orderService.ConfirmOrderAsync(orderId, voucherId);
                if (!confirmed) return BadRequest("Cannot confirm order");
                return Ok("Order confirmed");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId, string cancelReason)
        {
            try
            {
                var canceled = await _orderService.CancelOrderAsync(orderId, cancelReason);
                if (!canceled) return BadRequest("Cannot confirm order");
                return Ok("Order cancelled");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [HttpPatch("{orderId}/change-status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] int status)
        {
            try
            {
                bool isUpdate = await _orderService.UpdateOrderStatusAsync(orderId, status);

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
       

        [HttpGet("order-details")]
        public async Task<ActionResult<IEnumerable<GetOrderDetailDTO>>> GetAllOrderDetails()
        {
            var orderDetails = await _orderService.GetAllOrderDetailAsync();
            return Ok(orderDetails);
        }

        [HttpGet("order-details/{orderDetailId}")]
        public async Task<ActionResult<GetOrderDetailDTO>> GetOrderDetailById(int orderDetailId)
        {
            var orderDetail = await _orderService.GetOrderDetailByIdAsync(orderDetailId);
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
                bool isUpdate = await _orderService.UpdateOrderDetailStatusAsync(orderDetailId, status);

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
        

    }
}
