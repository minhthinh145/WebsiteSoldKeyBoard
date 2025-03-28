using KeyBoard.Helpers;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonService _service;

        public HoaDonController(IHoaDonService service) 
        {
            _service = service;
        }
        //get hoadon by userid
        [Authorize]
        [HttpGet("ByUser/{userId}")]
        public async Task<IActionResult> GetHoaDonsByUserId(string userId)
        {
            var hoaDonDTO = await _service.GetHoaDonsByUserIdAsync(userId);
            return Ok(hoaDonDTO);
        }
        //get hoadon by id
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHoaDonById(int id)
        {
            try
            {
                var hoaDonDTO = await _service.GetHoaDonByIdAsync(id);
                return Ok(hoaDonDTO);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        //add hoadon form OrderId
        [Authorize]
        [HttpPost("create-from-order/{orderId}")]
        public async Task<IActionResult> CreateHoaDonFromOrder(Guid orderId)
        {
            var result = await _service.CreateHoaDonFromOrderAsync(orderId);
            if (result > 0)
            {
                return Ok(new { Message = "Tạo hóa đơn thành công" });
            }
            return StatusCode(500);
        }

        //update status hoadon
        [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPut("update-status/{maHd}")]
        public async Task<IActionResult> UpdateStatusHoaDon(int maHd, [FromBody] int status)
        {
            var result = await _service.UpdatePaymentStatusAsync(maHd, status,DateTime.Now);
            if (result)
            {
                return Ok(new { Message = "Cập nhật trạng thái hóa đơn thành công" });
            }
            return StatusCode(500);
        }

        //delete hoadon by hoadon id
        [Authorize(Roles = ApplicationRole.Admin)]
        [HttpDelete("delete-byid/{maHd}")]
        public async Task<IActionResult> DeleteHoaDonById(int maHd) 
        {
            var result = await _service.DeleteHoaDonByIdAsync(maHd);
            if (result)
            {
                return Ok(new { Message = "Xóa hóa đơn thành công" });
            }
            return StatusCode(500);
        }
    }
}
