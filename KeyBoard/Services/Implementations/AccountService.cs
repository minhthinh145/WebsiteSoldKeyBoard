// KeyBoard.Services.Implementations/AccountService.cs
using KeyBoard.Data;
using KeyBoard.DTOs.AuthenDTOs;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using AutoMapper;
using KeyBoard.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace KeyBoard.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AccountService(UserManager<ApplicationUser> userManager, IMapper mapper, IAuthService authService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<ServiceResult> CheckPasswordAsync(string userId, string password)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return ServiceResult.Failure("Không tìm thấy người dùng với ID đã cho"); ;
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (isPasswordValid)
            {
                return ServiceResult.Success("Mật khẩu đúng");
            }
            else
            {
                return ServiceResult.Failure("Mật khẩu không đúng");
            }
        }


        public async Task<UserProfileDTO> FindUserById(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            if (user == null) return null!;

            return  _mapper.Map<UserProfileDTO>(user);
            
        }

        public async Task<TokenResponseDTO> SignInAsync(SignInDTO signin)
        {
            var user = await _userManager.FindByEmailAsync(signin.Email);
            if (user == null || !(await _userManager.CheckPasswordAsync(user, signin.Password))
)
            {
                return null!;
            }

            var (accessToken, refreshToken) = await _authService.GenerateTokensAsync(user);
            return new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<IdentityResult> SignUpAsync(SignUpDTO signup)
        {
            var user = new ApplicationUser
            {
                Email = signup.Email,
                UserName = signup.Username,
                PhoneNumber = signup.PhoneNumber,
                DateOfBirth = signup.DateOfBirth,
            };

            var result = await _userManager.CreateAsync(user, signup.Password);
            if (result.Succeeded)
            {
                await AssignCustomerRoleAsync(user);
            }

            return result;
        }

        public async Task<UserProfileDTO> UpdateUserById(string userId ,UserProfileDTO user)
        {
            var userApp = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (userApp == null)
            {
                return null!;
            }
            _mapper.Map(user, userApp);

            var result = await _userManager.UpdateAsync(userApp);

            return user;
            
        }

        private async Task AssignCustomerRoleAsync(ApplicationUser user)
        {
            var roleExists = await _userManager.GetRolesAsync(user);
            if (!roleExists.Contains(ApplicationRole.Customer))
            {
                await _userManager.AddToRoleAsync(user, ApplicationRole.Customer);
            }
        }
    }
}