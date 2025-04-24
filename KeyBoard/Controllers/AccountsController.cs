using KeyBoard.DTOs.AuthenDTOs;
using Microsoft.AspNetCore.Mvc;
using KeyBoard.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountsController( IAccountService service)
        {
            _service = service;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpDTO signup)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.SignUpAsync(signup);
                if (result.Succeeded)
                {
                    return Ok(result.Succeeded);
                }
                else
                {
                    return StatusCode(500, result.Errors);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi nội bộ khi xử lý đăng ký.");
            }
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInDTO signin)
        {
            var token = await _service.SignInAsync(signin);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            return Ok(token);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _service.FindUserById(userID);

            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
