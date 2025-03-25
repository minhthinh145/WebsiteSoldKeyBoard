using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddToCartAsync(Cart cart)
        {
            var existingCart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == cart.UserId && c.ProductId == cart.ProductId);

            if (existingCart != null)
            {
                existingCart.Quantity += cart.Quantity;
                _context.Carts.Update(existingCart); // Cần gọi Update() để EF Core theo dõi sự thay đổi
            }
            else
            {
                await _context.Carts.AddAsync(cart);
            }

            await _context.SaveChangesAsync(); // Lưu thay đổi vào database
        }


        public async Task ClearCartAsync(string userId)
        {
            var cartItems = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart?> GetCartItemAsync(string userId, Guid productId)
        {
           return await _context.Carts
                .Where(c => c.UserId == userId && c.ProductId == productId)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Cart>> GetCartItemsAsync(string userId)
        {
            return await _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToListAsync();
        }
        public async Task RemoveFromCartAsync(Cart cart)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
