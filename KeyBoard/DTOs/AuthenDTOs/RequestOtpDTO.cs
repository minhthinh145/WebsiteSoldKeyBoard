using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace KeyBoard.DTOs.AuthenDTOs
{
    public enum OtpDeliveryMethod
    {
        Email,
        Phone
    }
    public class RequestOtpDTO
    {

        [Required(ErrorMessage = "Delivery method is required")]
        public OtpDeliveryMethod Method { get; set; }
    }
}
