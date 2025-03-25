using KeyBoard.Data;
using KeyBoard.DTOs.AuthenDTOs;
using KeyBoard.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KeyBoard.Repositories.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<string> SignInAsync(SignInDTO signin)
        {
            var result = await _signInManager.PasswordSignInAsync(signin.Email, signin.Password, false, false);
            if (!result.Succeeded) 
            {
                return string.Empty;
            }

            var authClaims = new List<Claim>
            {  
                new Claim(ClaimTypes.Email, signin.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // tạo id cho token
            };

            var authenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(25),
                    claims: authClaims,
                    signingCredentials : new SigningCredentials(new SymmetricSecurityKey(authenKey), SecurityAlgorithms.HmacSha512Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }   

        public async Task<IdentityResult> SignUpAsync(SignUpDTO signup)
        {
            var user = new ApplicationUser
            {
                FirstName = signup.FirstName,
                LastName = signup.LastName,
                Email = signup.Email,
                UserName = signup.Email

            };
            return await _userManager.CreateAsync(user, signup.Password);
        }
    }
}
