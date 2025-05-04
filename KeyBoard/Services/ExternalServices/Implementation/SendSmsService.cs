using KeyBoard.Services.ExternalServices.Interface;
using Vonage;
using Vonage.Request;
using Vonage.Messaging;
using PhoneNumbers;
using CommandLine;
namespace KeyBoard.Services.ExternalServices.Implementation
{
    public class SendSmsService : ISendSmsService
    {
        private readonly IConfiguration _configuration;
        private readonly VonageClient _vonageClient;

        public SendSmsService(IConfiguration configuration)
        {
            _configuration = configuration;
            var credentials = Credentials.FromApiKeyAndSecret(
                _configuration["Vonage:ApiKey"],
                _configuration["Vonage:ApiSecret"]
                );
            _vonageClient = new VonageClient(credentials);
        }
        public async Task SendOtpSmsAsync(string phoneNumber, string otpCode)
        {
            try
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();
                var parsedNumber = phoneNumberUtil.Parse(phoneNumber, "VN");
                var formattedPhoneNumber = phoneNumberUtil.Format(parsedNumber, PhoneNumberFormat.E164);
                var senderId = _configuration["Vonage:SenderId"] ?? "YourApp";

                var response = await _vonageClient.SmsClient.SendAnSmsAsync(new SendSmsRequest
                {
                    To = formattedPhoneNumber,
                    From = senderId,
                    Text = $"Mã OTP của bạn là: {otpCode}"
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi gửi SMS: {ex.Message}");
            }
        }
    }
}
