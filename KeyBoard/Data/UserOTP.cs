using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyBoard.Data
{
    public class UserOTP
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string OtpCode{ get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsUsed { get; set; } = false;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }


    }
}
