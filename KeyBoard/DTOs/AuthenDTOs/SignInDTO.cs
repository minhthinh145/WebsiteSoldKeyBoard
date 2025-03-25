using System.ComponentModel.DataAnnotations;

namespace KeyBoard.DTOs.AuthenDTOs
{
    public class SignInDTO
    {
        [Required,EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
