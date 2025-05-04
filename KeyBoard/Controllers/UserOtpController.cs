using KeyBoard.DTOs.AuthenDTOs;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserOtpController : ControllerBase
    {
        public readonly IUserOtpService _userOtpService;
        public UserOtpController(IUserOtpService userOtpService)
        {
            _userOtpService = userOtpService;
        }
        [HttpPost("request")]
        public async Task<IActionResult> RequestOtp([FromBody] RequestOtpDTO requestDTO)
        {
            //claim
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return BadRequest("Không tìm thấy User");
            }
            requestDTO.UserId = userId;
            var result = await _userOtpService.GenerateOtpAsync(requestDTO.UserId, requestDTO.Method.ToString());
            if (!result)
            {
                return BadRequest("Gửi mã OTP không thành công");
            }
            return Ok("Gửi mã OTP thành công");
        }
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDTO verifyOtpDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest("Không tìm thấy User");
            }
            verifyOtpDTO.UserId = userId;
            var result = await _userOtpService.VerifyOtpAsync(verifyOtpDTO.UserId, verifyOtpDTO.OtpCode);
            if (!result)
            {
                return BadRequest("Mã OTP không hợp lệ hoặc đã hết hạn");
            }
            return Ok("Mã OTP hợp lệ");
        }
    }
}
