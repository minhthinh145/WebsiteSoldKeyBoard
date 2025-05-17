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
        [HttpGet("getCart")]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để xem giỏ hàng" });
            }

             var CartDTO = await _service.GetCartItemsAsync(userId);
            return Ok(CartDTO);
        }


        //add 
        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO cartDTO)
        {
            var userId = GetUserIdFromToken();
            if (userId == null) 
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để thêm sản phẩm vào giỏ hàng" });
            }
            var result = await _service.AddToCartAsync(userId,cartDTO);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        //update
        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateCart([FromBody] CartItemDTO cartDTO)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized(new { message = "Bạn cần đăng nhập để cập nhật giỏ hàng" });
            }
            var result = await _service.UpdateCartAsync(cartDTO,userId);
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId != null ? userId : null;
        }


    }
}
