using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Helpers;
using KeyBoard.Repositories.Interfaces;
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
        private readonly IOrderRepository _repo;
        private readonly IMapper _mapper;
        private readonly IProductRepository _product;
        private readonly ICartRepository _cart;

        public OrdersController(IOrderRepository repo , IMapper mapper, IProductRepository product , ICartRepository cart) 
        {
            _repo = repo;
            _mapper = mapper;
            _product = product;
            _cart = cart;
        }
        //get all orders
        [HttpGet]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> GetAllOrders() 
        {
            var orders = await _repo.GetAllOrdersAsync();
            if(orders == null)
            {
                return NotFound(new { message = "Orders not found" });
            }
            var ordersDTO = _mapper.Map<List<OrderDTO>>(orders);
            return Ok(ordersDTO);
        }

        //get oders by id
        [HttpGet("{id}")]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _repo.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound(new { message = "Order not found" });
            }
            var orderDTO = _mapper.Map<OrderDTO>(order);
            return Ok(orderDTO);
        }

        //get orders by user id
        [HttpGet("user/{userId}")]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> GetOrdersByUserId(string userId)
        {
            var orders = await _repo.GetOrdersByUserIdAsync(userId);
            if (orders == null)
            {
                return NotFound(new { message = "Orders not found" });
            }
            var ordersDTO = _mapper.Map<List<OrderDTO>>(orders);
            return Ok(ordersDTO);
        }

        //create order
        [HttpPost]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> CreateOrder([FromBody]OrderDTO orderDTO)
        {
            if(orderDTO == null)
            {
                return BadRequest(new { message = "Order cannot be null" });
            }
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User is not authenticated" });
            }
            var order = _mapper.Map<Order>(orderDTO);
            order.UserId = userId;

            try 
            {
                decimal total = 0;
                foreach(var orderDetail in order.OrderDetails)
                 {
                    var product = await _product.GetProductByIdAsync(orderDetail.ProductId);
                    if (product == null)
                    {
                        return BadRequest(new { message = $"Product {orderDetail.ProductId} not found." });
                    }
                    orderDetail.UnitPrice = product.Price; // Gán UnitPrice từ Product
                    total += orderDetail.UnitPrice * orderDetail.Quantity;
                }
                order.TotalAmount = total;
                    await _repo.CreateOrderAsync(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            var orderDTOresponse = _mapper.Map<OrderDTO>(order);
            return Ok(orderDTOresponse);
        }
        //update order status
        [HttpPut("{id}")]
        [Authorize(Roles = ApplicationRole.Customer)]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                return BadRequest(new { message = "Invalid order data" });
            }

            if (orderDTO.Id != id)
            {
                return BadRequest(new { message = "Id does not match" });
            }

            try
            {
                var updated = await _repo.UpdateOrderStatusAsync(id, orderDTO.OrderStatus);
                if (!updated)
                {
                    return NotFound(new { message = "Order not found" });
                }
                return Ok(new { message = "Order status updated successfully" });
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

            // Lấy danh sách sản phẩm trong giỏ hàng
            var cartItems = await _cart.GetCartItemsAsync(userId);
            if (cartItems == null || !cartItems.Any())
            {
                return BadRequest(new { message = "Your cart is empty" });
            }

            // Create a new Order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                OrderStatus = "Pending",
                OrderDetails = new List<OrderDetail>(),
                TotalAmount = 0
            };

            foreach (var cartItem in cartItems)
            {
                var product = await _product.GetProductByIdAsync(cartItem.ProductId);
                if (product == null)
                {
                    return BadRequest(new { message = $"Product {cartItem.ProductId} not found." });
                }

                var orderDetail = new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price
                };

                order.OrderDetails.Add(orderDetail);
                order.TotalAmount += orderDetail.UnitPrice * orderDetail.Quantity;
            }

            await _repo.CreateOrderAsync(order);
            await _cart.ClearCartAsync(userId); // Xóa giỏ hàng sau khi đặt hàng thành công
            var orderDTO = _mapper.Map<OrderDTO>(order);
            return Ok(new { message = "Order placed successfully", order = orderDTO });
        }

    }
}
