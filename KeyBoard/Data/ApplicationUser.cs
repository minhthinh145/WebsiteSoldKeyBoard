using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeyBoard.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
