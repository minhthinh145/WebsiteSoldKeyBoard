using AutoMapper;
using KeyBoard.DTOs.HoaDonsDTOs;
using KeyBoard.Helpers;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietHoaDonController : ControllerBase
    {
        private readonly IChiTietHoaDonService _service;

        public ChiTietHoaDonController(IChiTietHoaDonService service)
        {
            _service = service;
        }

        // Lấy danh sách ChiTietHoaDon theo mã Hóa Đơn
        [Authorize(Roles = $"{ApplicationRole.Customer},{ApplicationRole.Admin}")]
        [HttpGet("ByHoaDon/{maHd}")]
        public async Task<IActionResult> GetChiTietHoaDonsByHoaDonId(int maHd)
        {
            try
            {
                var chitiethoadons = await _service.GetChiTietHoaDonsByHoaDonIdAsync(maHd);
                return Ok(chitiethoadons);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Không tìm thấy chi tiết hóa đơn nào cho hóa đơn ID {maHd}.");
            }
        }

        // Lấy thông tin 1 ChiTietHoaDon theo mã ChiTietHoaDon
        [Authorize(Roles = $"{ApplicationRole.Customer},{ApplicationRole.Admin}")]
        [HttpGet("{chiTietId}")]
        public async Task<IActionResult> GetChiTietHoaDonById(int chiTietId)
        {
            try
            {
                var chiTiet = await _service.GetChiTietHoaDonsByChiTietHoaDonIdAsync(chiTietId);
                return Ok(chiTiet);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Không tìm thấy chi tiết hóa đơn với ID {chiTietId}.");
            }
        }

        // Cập nhật số lượng & đơn giá của một chi tiết hóa đơn
        [Authorize(Roles = ApplicationRole.Admin)]
        [HttpPut("{chiTietId}")]
        public async Task<IActionResult> UpdateChiTietHoaDon(int chiTietId, [FromBody] ChiTietHoaDonDTO dto)
        {
            if (dto == null) return BadRequest("Dữ liệu không hợp lệ.");

            try
            {
                var updated = await _service.UpdateChiTietHoaDonAsync(chiTietId, dto.SoLuong, dto.DonGia);
                if (!updated)
                    return BadRequest("Không có thay đổi nào được thực hiện.");

                return Ok("Cập nhật chi tiết hóa đơn thành công.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Không tìm thấy chi tiết hóa đơn với ID {chiTietId}.");
            }
        }

        // Xóa một chi tiết hóa đơn
        [Authorize(Roles = ApplicationRole.Admin)]
        [HttpDelete("{chiTietId}")]
        public async Task<IActionResult> DeleteChiTietHoaDon(int chiTietId)
        {
            var deleted = await _service.DeleteChiTietHoaDonAsync(chiTietId);
            if (!deleted)
                return NotFound($"Không tìm thấy chi tiết hóa đơn với ID {chiTietId}.");

            return Ok("Xóa chi tiết hóa đơn thành công.");
        }
    }
}
