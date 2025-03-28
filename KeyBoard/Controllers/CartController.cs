using KeyBoard.DTOs;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service) 
        {
            _service = service;
        }

        //get cart
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue("UserId");
            if (userId == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để xem giỏ hàng" });
            }

            var listCartDTO = await _service.GetCartItemsAsync(userId);
            return Ok(listCartDTO);
        }


        //add 
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] CartItemDTO cartDTO)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (userId == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng" });
            }
            cartDTO.UserId = userId;
            var result = await _service.AddToCartAsync(cartDTO); // Gọi Service

            if (!result)
            {
                return BadRequest(new { message = "Không thể thêm sản phẩm vào giỏ hàng" });
            }
            return Ok(new { message = "Sản phẩm đã được thêm vào giỏ hàng" });
        }

        //update
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateCart([FromBody] CartItemDTO cartDTO)
        {
            var userId = User.FindFirstValue("UserId");
            if (userId == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để cập nhật giỏ hàng" });
            }
            cartDTO.UserId = userId;
            var result = await _service.UpdateCartAsync(cartDTO);
            if (!result) 
            {
                return BadRequest(new { message = "Không thể cập nhật giỏ hàng" });
            }
            return Ok(new { message = "Giỏ hàng đã được cập nhật" });
        }


        //delete
        [Authorize]
        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromCart(Guid productId)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để xóa sản phẩm khỏi giỏ hàng" });
            }

            var result = await _service.RemoveFromCartAsync(userId, productId);
            if (!result)
            {
                return BadRequest(new { message = "Không thể xóa sản phẩm khỏi giỏ hàng" });
            }
            return Ok(new { message = "Sản phẩm đã được xóa khỏi giỏ hàng" });
        }

        //Clear
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để xóa giỏ hàng" });
            }

            var result = await _service.ClearCartAsync(userId);
            if (!result)
            {
                return BadRequest(new { message = "Không thể xóa giỏ hàng" });
            }

            return Ok(new { message = "Giỏ hàng đã được xóa" });
        }
        private string? GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            return userIdClaim != null ? userIdClaim.ToString() : null;
        }


    }
}
