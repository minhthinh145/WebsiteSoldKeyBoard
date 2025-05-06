namespace KeyBoard.Services.Interfaces
{
    public interface IUserOtpService
    {
        /// <summary>
        /// Generates an OTP and sends it to the user's email/phone.
        /// </summary>
        /// <param name="userId">The user ID for which the OTP will be generated</param>
        /// <returns>A Task containing the result of the OTP generation process.</returns>
        Task<bool> GenerateOtpAsync(string userId);

        /// <summary>
        /// Verifies the OTP entered by the user.
        /// </summary>
        /// <param name="otpCode">The OTP code entered by the user</param>
        /// <returns>true if OTP is valid, false otherwise.</returns>
        Task<bool> VerifyOtpAsync(string userId,string otpCode);

        /// <summary>
        /// Marks the OTP as used after verification.
        /// </summary>
        /// <param name="otpCode">The OTP code to be marked as used.</param>
        /// <returns>A Task indicating whether the OTP was successfully marked as used.</returns>
        Task<bool> MarkOtpAsUsedAsync(string userId,string otpCode);
    }
}
