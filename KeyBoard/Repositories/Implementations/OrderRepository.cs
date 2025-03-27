using KeyBoard.Data;
using KeyBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            order.Id = Guid.NewGuid();
            order.CreatedAt = DateTime.UtcNow;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId), "User Id cannot be null or empty.");
            }

            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(Guid orderId, string newStatus)
        {
            var affectedRows = await _context.Orders
                .Where(o => o.Id == orderId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(o => o.OrderStatus, newStatus));

            return affectedRows > 0;
        }
    }
}
