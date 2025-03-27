    using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Helpers;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [HttpGet("{id}")]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            try
            {
                var order = await _service.GetOrderByIdAsync(id);
                return Ok(order);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Order not found!" });
            }
        }

        //get orders by user id
        [HttpGet("user/{userId}")]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> GetOrdersByUserId()
        {
            var userId = User.FindFirst("UserId")?.Value; 
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
        [HttpPost]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> CreateOrder([FromBody]OrderDTO orderDTO)
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }
            var order  = await _service.CreateOrderAsync(orderDTO, userId);
            return Ok(order);
        }
        [HttpPut("{id}/status")]
        [Authorize(Roles = ApplicationRole.Admin)] // Chỉ Admin có quyền cập nhật
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
        [HttpPost("create-from-cart")]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> CreateOrderFromCart() 
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }

           var order =  await _service.CreateOrderFromCartAsync(userId);
            return Ok(order);
        }

    }
}
