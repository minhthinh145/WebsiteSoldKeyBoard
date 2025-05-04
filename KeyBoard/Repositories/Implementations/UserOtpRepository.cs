using KeyBoard.Data;
using KeyBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Repositories.Implementations
{
    public class UserOtpRepository : IUserOtpRepository
    {
        private readonly ApplicationDbContext _context;

        public UserOtpRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<UserOTP> CreateOTPAsync(UserOTP userDTO)
        {
            await _context.UserOtps.AddAsync(userDTO);
            await _context.SaveChangesAsync();
            return userDTO;
        }

        public async Task<UserOTP> GetOtpByCodeAsync(string otpCode)
        {
            return await _context.UserOtps
                .FirstOrDefaultAsync(u => u.OtpCode == otpCode && !u.IsUsed && u.ExpirationTime > DateTime.UtcNow);
        }

        public async Task<bool> MarkOtpAsUsedAsync(string userId, string otpCode)
        {
           var otp = await _context.UserOtps
                .FirstOrDefaultAsync(o => o.OtpCode == otpCode && o.UserId == userId);
            if (otp != null)
            {
                otp.IsUsed = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}   
