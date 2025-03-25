using KeyBoard.Repositories.Interfaces;
    using KeyBoard.DTOs.AuthenDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository repo;

        public AccountsController(IAccountRepository repo)
        {
            this.repo = repo;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDTO signup)
        {
            var result = await repo.SignUpAsync(signup);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }
            return StatusCode(500);
        }
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInDTO signin)
        {
            var token = await repo.SignInAsync(signin);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            return Ok(token);
        }
    }
}
