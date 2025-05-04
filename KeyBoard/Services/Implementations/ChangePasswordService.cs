using KeyBoard.Data;
using KeyBoard.DTOs.AuthenDTOs;
using KeyBoard.Helpers;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace KeyBoard.Services.Implementations
{
    public class ChangePasswordService : IChangePasswordService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;
        private readonly IUserOtpService _OtpService;

        public ChangePasswordService(UserManager<ApplicationUser> userManager, IPasswordValidator<ApplicationUser> passwordValidator , IUserOtpService otpService) 
        {
            _userManager = userManager;
            _passwordValidator = passwordValidator;
            _OtpService = otpService;
        }
        public async Task<ServiceResult> ConfirmChangePasswordAsync(ConfirmChangePasswordDTO ConfirmChangePasswordDTO)
        {
           var user = await _userManager.FindByIdAsync(ConfirmChangePasswordDTO.UserId);
            if (user == null)
            {
                return ServiceResult.Failure("Không tìm thấy User");
            }
           var validateResult = await _passwordValidator.ValidateAsync(_userManager ,user, ConfirmChangePasswordDTO.NewPassword);
            if (!validateResult.Succeeded)
            {
                var errors = string.Join("; ", validateResult.Errors.Select(e => e.Description));
                return  ServiceResult.Failure($"Mật khẩu mới không hợp lệ: {errors}");  
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, ConfirmChangePasswordDTO.NewPassword);
            if(!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return ServiceResult.Failure($"Đổi mật khẩu không thành công: {errors}");
            }
            //mark otp is used
            var otpResult = await _OtpService.MarkOtpAsUsedAsync(ConfirmChangePasswordDTO.UserId,ConfirmChangePasswordDTO.OtpCode);
            if (!otpResult)
            {
                return ServiceResult.Failure("Mã OTP không hợp lệ hoặc đã được sử dụng");
            }
            return ServiceResult.Success("Đổi mật khẩu thành công");
        }

        public async Task<ServiceResult> RequestChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userManager.FindByIdAsync(changePasswordDTO.UserId);
            if (user == null)
            {
                return ServiceResult.Failure("Không tìm thấy User");
            }
            if(changePasswordDTO.NewPassword != changePasswordDTO.ConfirmPassword)
            {
                return ServiceResult.Failure("Mật khẩu không khớp, vui lòng nhập lại!");
            }

            return ServiceResult.Success("Vui lòng chọn phương thức gửi mã OTP");
        }
    }
}
