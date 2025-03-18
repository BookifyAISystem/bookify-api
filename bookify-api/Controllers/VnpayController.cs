using AutoMapper;
using bookify_data.Entities;
using bookify_data.Enums;
using bookify_data.Helper;
using bookify_data.Model;
using bookify_service.Interfaces;
using bookify_service.Services;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;

namespace bookify_api.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class VnpayController : Controller
    {
        private readonly IVnpayService _vnpayService;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public VnpayController(IVnpayService vnpayService, IConfiguration configuration, IOrderService orderService, IMapper mapper)
        {
            _vnpayService = vnpayService;
            _configuration = configuration;
            _vnpayService.Initialize(_configuration["Vnpay:TmnCode"], _configuration["Vnpay:HashSecret"], _configuration["Vnpay:BaseUrl"], _configuration["Vnpay:ReturnUrl"]);
            _orderService = orderService;
            _mapper = mapper;
        }
        /// <summary>
        /// Tạo url thanh toán
        /// </summary>
        /// <param name="money">Số tiền phải thanh toán</param>
        /// <param name="description">Mô tả giao dịch</param>
        /// <returns></returns>

        [HttpGet("CreatePaymentUrlByOrder")]
        public async Task<ActionResult<string>> CreatePaymentUrlByOrder(int orderId)
        {
            try
            {
                var order = await _orderService.GetEntitesByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound("Không tìm thấy đơn hàng.");
                }
                if (order.Status != 1)
                {
                    return BadRequest("Đơn hàng không ở trạng thái chưa thanh toán.");
                }
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // Lấy địa chỉ IP của thiết bị thực hiện giao dịch
                //string description = "Thanh toán cho đơn hàng ${order.OrderId}";
                var request = new VnpayPaymentRequest
                {
                    PaymentId = DateTime.Now.Ticks,
                    Money = Convert.ToDouble(order.Total),
                    Description = $"{order.OrderId}" ,
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY, // Tùy chọn. Mặc định là tất cả phương thức giao dịch
                    CreatedDate = DateTime.Now, // Tùy chọn. Mặc định là thời điểm hiện tại
                    Currency = Currency.VND, // Tùy chọn. Mặc định là VND (Việt Nam đồng)
                    Language = DisplayLanguage.Vietnamese, // Tùy chọn. Mặc định là tiếng Việt
                };

                var paymentUrl = _vnpayService.GetPaymentUrl(request);

                return  Created(paymentUrl, paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CreatePaymentUrl")]
        public ActionResult<string> CreatePaymentUrl(double money, string description)
        {
            try
            {
                var ipAddress = NetworkHelper.GetIpAddress(HttpContext); // Lấy địa chỉ IP của thiết bị thực hiện giao dịch

                var request = new VnpayPaymentRequest
                {
                    PaymentId = DateTime.Now.Ticks,
                    Money = money,
                    Description = description,
                    IpAddress = ipAddress,
                    BankCode = BankCode.ANY, // Tùy chọn. Mặc định là tất cả phương thức giao dịch
                    CreatedDate = DateTime.Now, // Tùy chọn. Mặc định là thời điểm hiện tại
                    Currency = Currency.VND, // Tùy chọn. Mặc định là VND (Việt Nam đồng)
                    Language = DisplayLanguage.Vietnamese, // Tùy chọn. Mặc định là tiếng Việt
                    
                };

                var paymentUrl = _vnpayService.GetPaymentUrl(request);

                return Created(paymentUrl, paymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Thực hiện hành động sau khi thanh toán. URL này cần được khai báo với VNPAY để API này hoạt đồng (ví dụ: http://localhost:1234/api/Vnpay/IpnAction)
        /// </summary>
        /// <returns></returns>
        [HttpGet("IpnAction")]
        public async Task<IActionResult> IpnAction()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpayService.GetPaymentResult(Request.Query);
                    if (paymentResult.IsSuccess)
                    {
                        int orderId = int.Parse(paymentResult.Description);
                        if (orderId <= 0)
                        {
                            return BadRequest("Không xác định được OrderId từ dữ liệu thanh toán.");
                        }

                        await _orderService.UpdateOrderStatusAsync(orderId, 2);
                        // Thực hiện hành động nếu thanh toán thành công tại đây. Ví dụ: Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu.
                        return Ok();
                    }

                    // Thực hiện hành động nếu thanh toán thất bại tại đây. Ví dụ: Hủy đơn hàng.
                    return BadRequest("Thanh toán thất bại");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }

        /// <summary>
        /// Trả kết quả thanh toán về cho người dùng
        /// </summary>
        /// <returns></returns>
        [HttpGet("Callback")]
        public async Task<ActionResult<VnpayPaymentResult>> Callback()
        {
            if (Request.QueryString.HasValue)
            {
                try
                {
                    var paymentResult = _vnpayService.GetPaymentResult(Request.Query);

                    if (paymentResult.IsSuccess)
                    {

                        int orderId = int.Parse(paymentResult.Description);
                        if (orderId <= 0)
                        {
                            return BadRequest("Không xác định được OrderId từ dữ liệu thanh toán.");
                        }

                        await _orderService.UpdateOrderStatusAsync(orderId, 2);
                        return Ok(paymentResult);
                    }

                    return BadRequest(paymentResult);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            return NotFound("Không tìm thấy thông tin thanh toán.");
        }
    }
}
