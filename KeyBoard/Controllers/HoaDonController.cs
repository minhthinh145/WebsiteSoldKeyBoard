using AutoMapper;
using KeyBoard.DTOs.HoaDonsDTOs;
using KeyBoard.Repositories.Implementations;
using KeyBoard.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonRepository _repo;
        private readonly IMapper _mapper;

        public HoaDonController(IHoaDonRepository repo, IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }
        //get hoadon by userid
        [HttpGet("ByUser/{userId}")]
        public async Task<IActionResult> GetHoaDonsByUserId(string userId)
        {
            var hoadons = await _repo.GetHoaDonsByUserAsync(userId);
            if (hoadons == null)
            {
                return NotFound();
            }
            var hoadonsDTO = _mapper.Map<List<HoaDonDTO>>(hoadons);
            return Ok(hoadonsDTO);
        }
        //get hoadon by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHoaDonById(int id)
        {
            var hoadon = await _repo.GetHoaDonByIdAsync(id);
            if (hoadon == null)
            {
                return NotFound();
            }
            var hoaDonDTO = _mapper.Map<HoaDonDTO>(hoadon);
            return Ok(hoaDonDTO);
        }

        //add hoadon form OrderId
        [HttpPost("create-from-order/{orderId}")]
        public async Task<IActionResult> CreateHoaDonFromOrder(Guid orderId)
        {
            var maHd = await _repo.CreateHoaDonFromOrderAsync(orderId);
            return Ok(new { Message = "Hóa đơn được tạo thành công", MaHd = maHd });
        }

        //update status hoadon
        [HttpPut("update-status/{maHd}")]
        public async Task<IActionResult> UpdateStatusHoaDon(int maHd, [FromBody] int status)
        {
            var result = await _repo.UpdatePaymentStatusAsync(maHd, status,DateTime.Now);
            if (result)
            {
                return Ok(new { Message = "Cập nhật trạng thái hóa đơn thành công" });
            }
            return StatusCode(500);
        }
    }
}
