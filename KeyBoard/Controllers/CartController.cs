using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Helpers;
using KeyBoard.Repositories.Implementations;
using KeyBoard.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _repo;
        private readonly IMapper _mapper;

        public CartController(ICartRepository repo, IMapper mapper) 
        {
            _repo = repo;
            _mapper= mapper;
        }

        //get cart
        [HttpGet]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue("UserId");
            if (userId == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để xem giỏ hàng" });
            }

            var cartItems = await _repo.GetCartItemsAsync(userId);
            var cartDtos = _mapper.Map<List<CartItemDTO>>(cartItems);
            return Ok(cartDtos);
        }


        //add 
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDTO cartDTO)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng" });
            }

            var cart = _mapper.Map<Cart>(cartDTO);
            cart.Id = Guid.NewGuid();
            cart.UserId = userId.ToString();
            cart.CreatedAt = DateTime.UtcNow;

            await _repo.AddToCartAsync(cart);
            return Ok(new { message = "Sản phẩm đã được thêm vào giỏ hàng" });
        }

        //update
        [HttpPut]
        public async Task<IActionResult> UpdateCart([FromBody] CartItemDTO cartDTO)
        {
            var existingCart = await _repo.GetCartItemAsync(cartDTO.UserId, cartDTO.ProductId);
            if (existingCart == null)
            {
                return NotFound(new { message = "Sản phẩm không tồn tại trong giỏ hàng" });
            }
            existingCart.Quantity = cartDTO.Quantity; // Chỉ cần update số lượng

            await _repo.UpdateCartAsync(existingCart);
            return Ok(new { message = "Giỏ hàng đã được cập nhật" });
        }


        //delete
        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromCart(Guid productId)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để xóa sản phẩm khỏi giỏ hàng" });
            }

            var cart = await _repo.GetCartItemAsync(userId, productId);
            if (cart == null)
            {
                return NotFound(new { message = "Sản phẩm không tồn tại trong giỏ hàng" });
            }

            await _repo.RemoveFromCartAsync(cart);
            return Ok(new { message = "Sản phẩm đã được xóa khỏi giỏ hàng" });
        }

        //Clear
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để xóa giỏ hàng" });
            }

            await _repo.ClearCartAsync(userId);
            return Ok(new { message = "Giỏ hàng đã được xóa" });
        }
        private string? GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            return userIdClaim != null ? userIdClaim.ToString() : null;
        }


    }
}
