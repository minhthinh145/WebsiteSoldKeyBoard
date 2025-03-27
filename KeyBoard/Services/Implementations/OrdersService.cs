using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Services.Implementations
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrderRepository _repo;
        private readonly IMapper _mapper;
        private readonly ICartService _cart;
        private readonly IProductService _product;

        public OrdersService(IOrderRepository repo, ICartService cart, IProductService product ,IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _cart = cart;
            _product = product;
        }
        public async Task<OrderDTO> CreateOrderAsync(OrderDTO orderDTO, string userId)
        {
            if (orderDTO == null || orderDTO.OrderDetails == null || !orderDTO.OrderDetails.Any())
            {
                throw new ArgumentException("Invalid order data");
            }

            // Ánh xạ DTO sang entity Order
            var order = _mapper.Map<Order>(orderDTO);
            order.Id = Guid.NewGuid();
            order.UserId = userId;
            order.CreatedAt = DateTime.UtcNow;
            order.OrderStatus = "Pending"; 
            order.TotalAmount = 0;

            // Tính tổng tiền
            foreach (var detail in order.OrderDetails)
            {
                var product = await _product.GetProductByIdAsync(detail.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product {detail.ProductId} not found.");
                }
                detail.Id = Guid.NewGuid();
                detail.OrderId = order.Id;
                detail.UnitPrice = product.Price;
                order.TotalAmount += detail.UnitPrice * detail.Quantity;
            }

            // Lưu vào database
            await _repo.CreateOrderAsync(order);

            // Trả về OrderDTO
            return _mapper.Map<OrderDTO>(order);
        }


        public async Task<OrderDTO> CreateOrderFromCartAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated");
            }
            var cartItems = await _cart.GetCartItemsAsync(userId);
            if (cartItems == null || !cartItems.Any())
            {
                throw new InvalidOperationException("Your cart is empty");
            }
            var newOrder = new Order
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
                    throw new KeyNotFoundException($"Product {cartItem.ProductId} not found.");
                }

                var orderDetail = new OrderDetail { 
                    Id = Guid.NewGuid(),
                    OrderId = newOrder.Id,
                    ProductId = product.Id,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price,
                };

                newOrder.OrderDetails.Add(orderDetail);
                newOrder.TotalAmount += orderDetail.UnitPrice * orderDetail.Quantity;
            }
            
            await _repo.CreateOrderAsync(newOrder);
            return _mapper.Map<OrderDTO>(newOrder);
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _repo.GetAllOrdersAsync();
            if (orders == null) 
            {
                return new List<OrderDTO>();
            }
            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(Guid id)
        {
            var order = await _repo.GetOrderByIdAsync(id);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found!");
            }
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<List<OrderDTO>> GetOrdersByUserIdAsync(string userId)
        {
            var listOrders = await _repo.GetOrdersByUserIdAsync(userId);
            if (listOrders == null) 
            {
                return new List<OrderDTO>();
            }
            return _mapper.Map<List<OrderDTO>>(listOrders);
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, string newStatus)
        {
            var order = await _repo.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found");
            }

            if (!IsValidStatus(newStatus))
            {
                throw new ArgumentException("Invalid order status");
            }

            return await _repo.UpdateOrderStatusAsync(orderId, newStatus);
        }

        // Kiểm tra trạng thái hợp lệ
        private bool IsValidStatus(string status)
        {
            var validStatuses = new List<string> { "Pending", "Processing", "Shipped", "Completed", "Canceled" };
            return validStatuses.Contains(status);
        }
    }
}
