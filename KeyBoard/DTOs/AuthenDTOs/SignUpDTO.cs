using System.ComponentModel.DataAnnotations;

namespace KeyBoard.DTOs.AuthenDTOs
{
    public class SignUpDTO
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
  
        [Required]
        public string ConfirmPassword {  get; set; } = string.Empty;


    }
}
