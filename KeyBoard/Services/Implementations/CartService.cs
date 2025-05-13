using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Helpers;
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
        public async Task<ServiceResult> AddToCartAsync(string userId, AddToCartDTO cartDTO)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return ServiceResult.Failure("Lỗi user 404");
            }
            var Cart = _mapper.Map<Cart>(cartDTO);
            Cart.UserId = userId;
            var existingCart = await _repo.GetCartItemAsync(userId, Cart.ProductId);
            if (existingCart != null)
            {
                // Nếu có, kiểm tra và cập nhật số lượng
                if (existingCart.Quantity + cartDTO.Quantity > 0) // Đảm bảo không giảm số lượng dưới 1
                {
                    existingCart.Quantity += cartDTO.Quantity;
                    await _repo.UpdateCartAsync(existingCart);
                    return ServiceResult.Success("Cập nhật số lượng món trong giỏ hàng thành công");
                }
                else
                {
                    return ServiceResult.Failure("Số lượng sản phẩm không hợp lệ");
                }
            }
            else
            {
                Cart.Id = Guid.NewGuid();
                Cart.CreatedAt = DateTime.UtcNow;
                await _repo.AddToCartAsync(Cart);


            }
            return ServiceResult.Success("Thêm vào giỏ thành công");
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

        public async Task<CartDTO> GetCartItemsAsync(string userId)
        {
            var listCart = await _repo.GetCartItemsAsync(userId);
            if (listCart == null || !listCart.Any())
            {
                return new CartDTO();
            }
           
            var items = _mapper.Map<List<CartItemDTO>>(listCart);
            var totalPrice = items.Sum(item => item.Price * item.Quantity);

            var CartDTO = new CartDTO
            {
                Items = items,
                TotalPrice = totalPrice,
            };
            return CartDTO;
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

        public async Task<bool> UpdateCartAsync(CartItemDTO cartDTO, string userId)
        {
            var existingCart = await _repo.GetCartItemAsync(userId, cartDTO.ProductId);
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
