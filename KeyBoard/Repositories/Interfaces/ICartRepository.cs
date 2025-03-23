using KeyBoard.Data;
using KeyBoard.DTOs;

namespace KeyBoard.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<List<CartItemDTO>> GetCartItemsAsync(Guid userId);
        Task<Cart?> GetCartItemAsync(Guid userId, Guid productId);
        Task AddToCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task RemoveFromCartAsync(Cart cart);
        Task ClearCartAsync(Guid userId);
        Task SaveChangesAsync();
    }
}
