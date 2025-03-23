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
            }
            else
            {
                await _context.Carts.AddAsync(cart); 
            }
            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cartItems = await _context.Carts.Where(c => c.UserId == userId).ToListAsync();
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart?> GetCartItemAsync(Guid userId, Guid productId)
        {
           return await _context.Carts
                .Where(c => c.UserId == userId && c.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<CartItemDTO>> GetCartItemsAsync(Guid userId)
        {
            var cartItems = _context.Carts
                .Where(c => c.UserId == userId)
                .Include(c=> c.Product)// load infor sản phẩm
                .Select( c=> new CartItemDTO
                {
                    ProductId = c.ProductId,
                    ProductName = c.Product.Name,
                    Quantity = c.Quantity,
                    Price = c.Product.Price,
                    ImageUrl = c.Product.ImageUrl!,
                    CreatedAt = c.CreatedAt ?? DateTime.Now
                }).ToListAsync();
            return await cartItems;
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
