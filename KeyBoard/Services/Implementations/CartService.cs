using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.Interfaces;

namespace KeyBoard.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repo;
        private readonly IMapper _mapper;

        public CartService(ICartRepository repo, IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<bool> AddToCartAsync(CartItemDTO cartDTO)
        {
            var Cart = _mapper.Map<Cart>(cartDTO);
            var existingCart = await _repo.GetCartItemAsync(Cart.UserId, Cart.ProductId);
            if (existingCart != null)
            {
                existingCart.Quantity += cartDTO.Quantity;
                await _repo.UpdateCartAsync(Cart);
            }
            else 
            {
                Cart.Id = Guid.NewGuid();
                Cart.CreatedAt = DateTime.UtcNow;
                await _repo.AddToCartAsync(Cart);
            }
            return true;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var listCart = await _repo.GetCartItemsAsync(userId);
            if (listCart == null || !listCart.Any())
            {
                return false;
            }
            await _repo.ClearCartAsync(userId);
            return true;
        }

        public async Task<List<CartItemDTO>> GetCartItemsAsync(string userId)
        {
            var listCart = await _repo.GetCartItemsAsync(userId);
            if (listCart == null || !listCart.Any())
            {
                return new List<CartItemDTO>();
            }
            else 
            {
                return _mapper.Map<List<CartItemDTO>>(listCart);
            }
        }

        public async Task<bool> RemoveFromCartAsync(string userId, Guid productId)
        {
            var existingCart = await _repo.GetCartItemAsync(userId, productId);
            if (existingCart == null)
            {
                return false;
            }
            await _repo.RemoveFromCartAsync(existingCart);
            return true;
        }

        public async Task<bool> UpdateCartAsync(CartItemDTO cartDTO)
        {
            var existingCart = await _repo.GetCartItemAsync(cartDTO.UserId, cartDTO.ProductId);
            if (existingCart == null)
            {
                return false;
            }
            existingCart.Quantity = cartDTO.Quantity;
            await _repo.UpdateCartAsync(existingCart);

            return true;
        }

    }
}
