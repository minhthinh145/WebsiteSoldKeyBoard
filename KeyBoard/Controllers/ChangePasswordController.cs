using KeyBoard.DTOs.AuthenDTOs;
using KeyBoard.Helpers;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Diagnostics.Tracing.Parsers.MicrosoftWindowsTCPIP;
using System.Security.Claims;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePasswordController : ControllerBase
    {
        private readonly IChangePasswordService _service;

        public ChangePasswordController(IChangePasswordService service)
        {
            _service = service;
        }
        [HttpPost("request")]
        public async Task<IActionResult> RequestChangePassword([FromBody] ChangePasswordDTO requestDTO)
        {
            //use claim
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return BadRequest("Có lỗi xảy ra");
            }
            requestDTO.UserId = userId;
            var result = await _service.RequestChangePasswordAsync(requestDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result); 
        }
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmChangePassword([FromBody] ConfirmChangePasswordDTO requestDTO)
        {
            //use claim
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest("Có lỗi xảy ra");
            }
            var result = await _service.ConfirmChangePasswordAsync(userId,requestDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }
    }
}
