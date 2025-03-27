using KeyBoard.DTOs.VNPayDTOs;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.VNPayServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static KeyBoard.Helpers.VNPayHelper;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPayController : ControllerBase
    {
        private readonly IHoaDonRepository _hoadon;
        private readonly IVNPayService _vnPayService;

        public VNPayController(IVNPayService vnPayService , IHoaDonRepository hoadon)
        {
            _hoadon = hoadon;
            _vnPayService = vnPayService;
        }
        /// <summary>
        /// API tạo URL thanh toán VNPay
        /// </summary>
        [HttpPost("create-payment-url/{maHD}")]
        public async Task<IActionResult> CreatePaymentUrl([FromBody] int maHD)
        {

            var paymentUrl = _vnPayService.CreatePaymentUrl(maHD, HttpContext);
           
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
