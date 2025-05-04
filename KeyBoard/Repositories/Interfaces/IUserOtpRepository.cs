using KeyBoard.Data;

namespace KeyBoard.Repositories.Interfaces
{
    public interface IUserOtpRepository
    {
        /// <summary>
        /// Creates a new OTP (One-Time Password) for a user.
        /// </summary>
        /// <param name="userDTO">DTO from frontend</param>
        /// <returns>UserOTP if success</returns>
        Task<UserOTP> CreateOTPAsync(UserOTP userDTO);

        /// <summary>
        /// Retrieves an OTP by its code.
        /// </summary>
        /// <param name="otpCode"></param>
        /// <returns>UserOTP if sucess</returns>
        Task<UserOTP> GetOtpByCodeAsync(string otpCode);

        /// <summary>
        /// Retrieves an OTP by its ID.
        /// </summary>
        /// <param name="otpCode"></param>
        /// <returns>true if otp is used else false</returns>
        Task<bool> MarkOtpAsUsedAsync(string userId,string otpCode);


    }
}
