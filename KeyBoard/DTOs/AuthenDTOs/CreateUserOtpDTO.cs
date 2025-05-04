namespace KeyBoard.DTOs.AuthenDTOs
{
    public class CreateUserOtpDTO
    {
        public string UserId { get; set; }
        public string OtpCode { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
