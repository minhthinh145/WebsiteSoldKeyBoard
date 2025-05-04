namespace KeyBoard.Services.ExternalServices.Interface
{
    public interface ISendSmsService
    {
        /// <summary>
        /// Send OTP to User ( by SMS )
        /// </summary>
        /// <param name="phoneNumber">Số điện thoại nhận OTP (định dạng quốc tế)</param>
        /// <param name="otpCode">Mã OTP cần gửi</param>
        Task SendOtpSmsAsync(string phoneNumber, string otpCode);
    }
}
