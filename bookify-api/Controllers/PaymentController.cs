using bookify_data.Model;
using bookify_service.Interfaces;
using bookify_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{
    [Route("api/v1/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPaymentDTO>>> GetAllPayments()
        {
            var orders = await _paymentService.GetAllPaymentAsync();
            return Ok(orders);
        }

        [HttpGet("{status}")]
        public async Task<ActionResult<IEnumerable<GetPaymentDTO>>> GetAllPaymentsByStatus(int status)
        {
            var orders = await _paymentService.GetPaymentsByStatusAsync(status);
            return Ok(orders);
        }


        [HttpGet("{accountId}")]
        public async Task<ActionResult<IEnumerable<GetPaymentDTO>>> GetPaymentsByAccountId(int accountId)
        {
            var orders = await _paymentService.GetPaymentsByAccountIdAsync(accountId);
            return Ok(orders);
        }
        [HttpGet("{paymentId}")]
        public async Task<ActionResult<GetPaymentDTO>> GetPaymentById(int paymentId)
        {
            var order = await _paymentService.GetPaymentByIdAsync(paymentId);
            if (order == null)
                return NotFound(new { message = "Order not found" });

            return Ok(order);
        }
        [HttpPost("{orderId}")]
        public async Task<ActionResult> CreatePayment(int orderId)
        {
            bool isCreated = await _paymentService.CreateCODPaymentAsync(orderId);
            if (!isCreated)
                return BadRequest(new { message = "Failed to create payment" });

            return StatusCode(201, new { message = "Payment created successfully" });
        }
    }
}
