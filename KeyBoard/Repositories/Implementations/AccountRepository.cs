using KeyBoard.Data;
using KeyBoard.DTOs.AuthenDTOs;
using KeyBoard.Helpers;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, RoleManager<IdentityRole> roleManager) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }
        public async Task<string> SignInAsync(SignInDTO signin)
        {
            //check user exist
            var user = await _userManager.FindByEmailAsync(signin.Email);
            var passwordValid = await _userManager.CheckPasswordAsync(user, signin.Password);
            if (user == null || !passwordValid)
            {
                return string.Empty;
            }

            var authClaims = new List<Claim>
            {  
                new Claim(ClaimTypes.Email, signin.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // tạo id cho token
                new Claim("UserId",user.Id.ToString())
            };
            //add role for user
            var userRole = await _userManager.GetRolesAsync(user);
            foreach (var role in userRole)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            //create token
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
            var result = await _userManager.CreateAsync(user, signup.Password);

            //add role default for user
            if (result.Succeeded) 
            {
                //check role customer exist ?
                if(!await _roleManager.RoleExistsAsync(ApplicationRole.Customer))
                {
                    await _roleManager.CreateAsync(new IdentityRole(ApplicationRole.Customer));
                }
                await _userManager.AddToRoleAsync(user, ApplicationRole.Customer);
            }

            return result;
        }
    }
}
