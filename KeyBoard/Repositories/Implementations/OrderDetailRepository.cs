using KeyBoard.Data;
using KeyBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Repositories.Implementations
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<bool> AddOrderDetailAsync(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
            var saved = await _context.SaveChangesAsync();

            return saved > 0; 
        }

        public async Task<OrderDetail?> GetOrderDetailByIdAsync(Guid orderDetailId)
        {
            var orderDetail = await _context.OrderDetails
                .Where(od => od.Id == orderDetailId)
                .Include(od => od.Product)
                .FirstOrDefaultAsync();
            return orderDetail;
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId)
        {
            var orderDetails = await _context.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Product)
                .ToListAsync();
            return orderDetails;
        }

        public async Task<bool> RemoveOrderDetailAsync(Guid orderDetailId)
        {
            var oderdetail = await _context.OrderDetails.FindAsync(orderDetailId);
            _context.OrderDetails.Remove(oderdetail);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            var exsitOrderDetail = await _context.OrderDetails.FindAsync(orderDetail.Id);
            if (exsitOrderDetail == null)
            {
                return false;
            }
            _context.Entry(exsitOrderDetail).CurrentValues.SetValues(orderDetail);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
