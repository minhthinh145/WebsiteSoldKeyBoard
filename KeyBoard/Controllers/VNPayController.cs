using KeyBoard.DTOs.VNPayDTOs;
using KeyBoard.Services.VNPayServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static KeyBoard.Helpers.VNPayHelper;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPayController : ControllerBase
    {
        private readonly IVNPayService _vnPayService;

        public VNPayController(IVNPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }
        /// <summary>
        /// API tạo URL thanh toán VNPay
        /// </summary>
        [HttpPost("create-payment-url")]
        public IActionResult CreatePaymentUrl([FromBody] VNPayRequestDTO request)
        {
            var paymentUrl = _vnPayService.CreatePaymentUrl(request, HttpContext);
           
            return Ok(new { url = paymentUrl });
        }

        [HttpGet("vnpay-return")]
        public IActionResult ProcessPaymentResponse()
        {
         
            if (HttpContext.Request.Query.Count == 0)
            {
                return BadRequest("Query parameters are missing.");
            }

            try
            {
                var response = _vnPayService.ProcessPaymentResponse(HttpContext.Request.Query);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi xử lý phản hồi thanh toán.");
            }
        }
    }
}
