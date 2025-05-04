using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs.AuthenDTOs;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.ExternalServices.Interface;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace KeyBoard.Services.Implementations
{
    public class UserOtpService : IUserOtpService
    {
        private readonly ISendEmailService _sendEmailService;
        private readonly ISendSmsService _sendSmsService;
        private readonly IUserOtpRepository _userOtpRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly int _otpExpirationTime = 15; // Thời gian hết hạn OTP (tính bằng phút)
        public UserOtpService(
            ISendEmailService sendEmailService,
            ISendSmsService sendSmsService,
            IUserOtpRepository userOtpRepository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _sendEmailService = sendEmailService;
            _sendSmsService = sendSmsService;
            _userOtpRepository = userOtpRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> GenerateOtpAsync(string userId , string method)
        {
            try
            {
                if (method != "Email" && method != "Phone")
                {
                    return false; // Phương thức không hợp lệ
                }
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return false;
                }
                var otpCode = GenerateOtpCode();
                var otpDTO = new CreateUserOtpDTO {
                    UserId = userId,
                    OtpCode = otpCode,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(_otpExpirationTime)
                };
                var userOtp = _mapper.Map<UserOTP>(otpDTO);

                await _userOtpRepository.CreateOTPAsync(userOtp);

                if (method == "Email")
                {
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        return false; 
                    }
                    await _sendEmailService.SendOtpEmailAsync(user.Email, otpCode);
                }
                else // method == "Phone"
                {
                    if (string.IsNullOrEmpty(user.PhoneNumber))
                    {
                        return false; // Số điện thoại không tồn tại
                    }
                    await _sendSmsService.SendOtpSmsAsync(user.PhoneNumber, otpCode);
                }
                return true;
            }
            catch (Exception)
            {
                return false; 
            }
        }

        public async Task<bool> MarkOtpAsUsedAsync(string userId,string otpCode)
        {
            try
            {
                return await _userOtpRepository.MarkOtpAsUsedAsync(userId,otpCode);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> VerifyOtpAsync(string userId,string otpCode)
        {
            try
            {
                var userOTP = await _userOtpRepository.GetOtpByCodeAsync(otpCode);
                if (userOTP == null || userOTP.UserId != userId || userOTP.IsUsed || userOTP.ExpirationTime < DateTime.UtcNow)
                {
                    return false;
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private string GenerateOtpCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }   
    }
}
