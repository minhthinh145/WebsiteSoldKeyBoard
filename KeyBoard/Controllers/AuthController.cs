// KeyBoard.Controllers/AuthController.cs
using KeyBoard.DTOs.AuthenDTOs;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;

        public AuthController(IAccountService accountService, IAuthService authService)
        {
            _accountService = accountService;
            _authService = authService;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signIn)
        {
            var result = await _accountService.SignInAsync(signIn);
            if (result == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            return Ok(result);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUp)
        {
            var result = await _accountService.SignUpAsync(signUp);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Sign up failed.", errors = result.Errors });
            }

            return Ok(new { message = "Sign up successful." });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { message = "Refresh token is required." });
            }

            try
            {
                var accessToken = await _authService.RefreshAccessTokenAsync(request.RefreshToken);
                return Ok(new { AccessToken = accessToken });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody] RefreshTokenRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { message = "Refresh token is required." });
            }

            await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
            return Ok(new { message = "Signed out successfully." });
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            var userProfile = await _accountService.FindUserById(userId);
            if (userProfile == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(userProfile);
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UserProfileDTO user)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new { message = "Invalid token." });
            }
            var userUpdate = await _accountService.UpdateUserById(userId, user);
            if (userUpdate == null)
            {
                return BadRequest(new { message = "Cannot update user" });
            }
            return Ok(new { message = "Cập nhật thông tin thành công" }); // Trả về thông báo thay vì DTO        
        }
    }
}