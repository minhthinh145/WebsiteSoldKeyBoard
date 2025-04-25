// KeyBoard.Services.Implementations/AccountService.cs
using KeyBoard.Data;
using KeyBoard.DTOs.AuthenDTOs;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using AutoMapper;
using KeyBoard.Helpers;

namespace KeyBoard.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AccountService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper, IAuthService authService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<UserProfileDTO> FindUserById(string userID)
        {
            var user = await _userManager.FindByIdAsync(userID);
            return user == null ? null : _mapper.Map<UserProfileDTO>(user);
        }

        public async Task<TokenResponseDTO> SignInAsync(SignInDTO signin)
        {
            var user = await _userManager.FindByEmailAsync(signin.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, signin.Password))
            {
                return null;
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
            };

            var result = await _userManager.CreateAsync(user, signup.Password);
            if (result.Succeeded)
            {
                await AssignCustomerRoleAsync(user);
            }

            return result;
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