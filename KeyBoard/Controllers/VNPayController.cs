using KeyBoard.DTOs.VNPayDTOs;
using KeyBoard.Services.VNPayServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// API xử lý phản hồi từ VNPay
        /// </summary>
        [HttpGet("payment-response")]
        public IActionResult ProcessPaymentResponse()
        {
            var response = _vnPayService.ProcessPaymentResponse(Request.Query);
            return Ok(response);
        }

        [HttpGet("payment-return")]
        public IActionResult VNPayReturn()
        {
            var query = HttpContext.Request.Query;//Lấy query từ request
            var response = _vnPayService.ProcessPaymentResponse(query);

            return Ok(response);
        }

    }
}
