using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs.HoaDonsDTOs;
using KeyBoard.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietHoaDonController : ControllerBase
    {
        private readonly IChiTietHoaDonRepository _repo;
        private readonly IMapper _mapper;

        public ChiTietHoaDonController(IChiTietHoaDonRepository repo , IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        //get chitiethoadon by hoadonid
        [HttpGet("ByHoaDon/{maHd}")]
        public async Task<IActionResult> GetChiTietHoaDonsByHoaDonId(int maHd)
        {
            var chitiethoadons = await _repo.GetByHoaDonIdAsync(maHd);
            if (chitiethoadons == null)
            {
                return NotFound();
            }
            return Ok(chitiethoadons);
        }

       

    }
}
