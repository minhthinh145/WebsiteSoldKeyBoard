using Microsoft.AspNetCore.Identity;

namespace KeyBoard.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
        public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    }
}
