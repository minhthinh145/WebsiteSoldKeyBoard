using AutoMapper;
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
        private readonly IMapper _mapper;

        public AccountService(IAccountRepository repo, IConfiguration configuration, IMapper mapper) 
        {
            _mapper = mapper;
            _repo = repo;
            _configuration = configuration;
        }

        public async Task<UserProfileDTO> FindUserById(string userID)
        {
            var user = await _repo.FindByUserIDAsync(userID);
            return user == null ? null : _mapper.Map<UserProfileDTO>(user);
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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("Username", user.UserName)
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
                Email = signup.Email,
                UserName = signup.Username,
                FirstName = signup.Username,
                LastName = signup.Username,
                PhoneNumber = signup.PhoneNumber,

            };

            // Tạo người dùng mới
            var result = await _repo.CreateUserAsync(user, signup.Password);


            // Nếu thành công, kiểm tra vai trò
            if (!await _repo.RoleExistsAsync(ApplicationRole.Customer))
            {
                await _repo.CreateRoleAsync(ApplicationRole.Customer);
            }

            // Thêm người dùng vào vai trò
            await _repo.AddToRoleAsync(user, ApplicationRole.Customer);

            return result; // Trả về kết quả thành công
        }

    }
}
