using KeyBoard.Data;
using KeyBoard.DTOs.AuthenDTOs;
using KeyBoard.Helpers;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KeyBoard.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        private readonly IConfiguration _configuration;


        public AccountService( IAccountRepository repo , IConfiguration configuration) 
        {
            _repo = repo;
            _configuration = configuration;
        }

        public async Task<string> SignInAsync(SignInDTO signin)
        {
            var user = await _repo.FindByEmailAsync(signin.Email);
            if (user == null || !await _repo.CheckPasswordAsync(user, signin.Password))
            {
                return string.Empty;
            }
            //create authClaims
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, signin.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Token ID
                new Claim("UserId", user.Id.ToString())
            };

            //add role
            var userRole = await _repo.GetUserRolesAsync(user);
            foreach (var role in userRole)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            //create token
            var authKey = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(55),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(authKey), SecurityAlgorithms.HmacSha512Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpDTO signup)
        {
            var user = new ApplicationUser
            {
                UserName = signup.Email,
                Email = signup.Email,
                FirstName = signup.FirstName,
                LastName = signup.LastName
            };
            var result = await _repo.CreateUserAsync(user, signup.Password);
            if(result.Succeeded)
            {
                if(!await _repo.RoleExistsAsync(ApplicationRole.Customer))
                {
                    await _repo.CreateRoleAsync(ApplicationRole.Customer);
                }
                await _repo.AddToRoleAsync(user, ApplicationRole.Customer);
            }

            return result;
        }
    }
}
