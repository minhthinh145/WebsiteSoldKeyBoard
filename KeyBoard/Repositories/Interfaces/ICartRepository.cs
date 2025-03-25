using KeyBoard.Data;
using KeyBoard.DTOs;

namespace KeyBoard.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<List<Cart>> GetCartItemsAsync(string userId);
        Task<Cart?> GetCartItemAsync(string userId, Guid productId);
        Task AddToCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task RemoveFromCartAsync(Cart cart);
        Task ClearCartAsync(string userId); 
        Task SaveChangesAsync();
    }
}
