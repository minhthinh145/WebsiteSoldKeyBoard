using KeyBoard.Data;
using KeyBoard.DTOs;

namespace KeyBoard.Services.Interfaces
{
    public interface IOrderDetailsService
    {
        /// <summary>
        /// Lấy danh sách chi tiết đơn hàng theo OrderId.
        /// </summary>
        Task<List<OrderDetailDTO>> GetOrderDetailsByOrderIdAsync(Guid orderId);

        /// <summary>
        /// Lấy một chi tiết đơn hàng cụ thể theo OrderDetailId.
        /// </summary>
        Task<OrderDetailDTO?> GetOrderDetailByIdAsync(Guid orderDetailId);

        /// <summary>
        /// Thêm một chi tiết đơn hàng mới.
        /// </summary>
        Task<bool> AddOrderDetailAsync(OrderDetailDTO orderDetailDTO);

        /// <summary>
        /// Cập nhật thông tin một chi tiết đơn hàng.
        /// </summary>
        Task<bool> UpdateOrderDetailAsync(OrderDetailDTO orderDetailDTO);

        /// <summary>
        /// Xóa một chi tiết đơn hàng theo OrderDetailId.
        /// </summary>
        Task<bool> RemoveOrderDetailAsync(Guid orderDetailId);

        /// <summary>
        /// Tính tổng tiền của một đơn hàng dựa trên danh sách chi tiết đơn hàng.
        /// </summary>
        Task<decimal> GetTotalPriceByOrderIdAsync(Guid orderId);

        /// <summary>
        /// Kiểm tra xem một sản phẩm có tồn tại trong đơn hàng không.
        /// </summary>
        Task<bool> CheckProductInOrderAsync(Guid orderId, Guid productId);
    }
}
