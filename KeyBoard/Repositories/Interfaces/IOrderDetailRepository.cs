using KeyBoard.Data;

namespace KeyBoard.Repositories.Interfaces
{
    public interface IOrderDetailRepository
    {
        Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(Guid orderId);
        Task<OrderDetail?> GetOrderDetailByIdAsync(Guid orderDetailId);
        Task<bool> AddOrderDetailAsync(OrderDetail orderDetail);
        Task<bool> UpdateOrderDetailAsync(OrderDetail orderDetail);
        Task<bool> RemoveOrderDetailAsync(Guid orderDetailId);
    }
}
