using System.ComponentModel.DataAnnotations;

namespace KeyBoard.DTOs.AuthenDTOs
{
    public class ConfirmChangePasswordDTO
    {
        public string UserId { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        public string NewPassword { get; set; }
        public string OtpCode { get; set; }
    }
}

