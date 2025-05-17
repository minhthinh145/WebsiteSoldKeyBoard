using KeyBoard.DTOs;
using KeyBoard.Helpers;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KeyBoard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _service;

        public OrdersController(IOrdersService service)
        {
            _service = service;
        }
        //get all orders
        [HttpGet]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _service.GetAllOrdersAsync();
            return Ok(orders);
        }

        //get oders by id
        [HttpGet("getOrder")]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> GetOrderById([FromQuery] Guid id)
        {
            var userId = GetUserIdByToken();
            var result = await _service.GetOrderByIdAsync(id, userId);

            if (!result.IsSuccess)
            {
                return result.Message.Contains("not found")
                    ? NotFound(new { result.Message })
                    : StatusCode(403, new { result.Message });
            }

            return Ok(new { result.Data });
        }

        //get orders by user id
        [HttpGet("user/{userId}")]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> GetOrdersByUserId()
        {
            var userId = GetUserIdByToken();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }

            var orders = await _service.GetOrdersByUserIdAsync(userId);
            if (!orders.Any()) // Kiểm tra rỗng
            {
                return NotFound(new { message = "No orders found for this user." });
            }

            return Ok(orders);
        }

        //create order
        [HttpPost("order")]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDTO orderDTO)
        {
            var userId = GetUserIdByToken();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }
            var order = await _service.CreateOrderAsync(orderDTO, userId);
            return Ok(order);
        }
        [HttpPut("{id}/status")]
        [Authorize(Roles = ApplicationRole.Admin)] 
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] string newStatus)
        {
            if (string.IsNullOrEmpty(newStatus))
            {
                return BadRequest(new { message = "Order status is required." });
            }

            try
            {
                var updated = await _service.UpdateOrderStatusAsync(id, newStatus);
                if (!updated)
                {
                    return NotFound(new { message = "Order not found." });
                }
                return Ok(new { message = "Order status updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //create ORders from carts
        [HttpPost("createorder")]
        [Authorize]
        public async Task<IActionResult> CreateOrderFromCart()
        {
            var userId = GetUserIdByToken();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }

            var order = await _service.CreateOrderFromCartAsync(userId);
            return Ok(order);
        }

        private string GetUserIdByToken()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return userId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
