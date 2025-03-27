using KeyBoard.DTOs;

namespace KeyBoard.Services.Interfaces
{
    public interface IOrdersService
    {
        /// <summary>
        /// Lấy danh sách tất cả đơn hàng.
        /// </summary>
        /// <returns>Danh sách OrderDTO</returns>
        Task<List<OrderDTO>> GetAllOrdersAsync();

        /// <summary>
        /// Lấy thông tin chi tiết của một đơn hàng theo ID.
        /// </summary>
        /// <param name="id">ID của đơn hàng</param>
        /// <returns>OrderDTO nếu tìm thấy, ngược lại trả về null</returns>
        Task<OrderDTO> GetOrderByIdAsync(Guid id);

        /// <summary>
        /// Lấy danh sách đơn hàng theo ID người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Danh sách OrderDTO</returns>
        Task<List<OrderDTO>> GetOrdersByUserIdAsync(string userId);

        /// <summary>
        /// Tạo đơn hàng mới từ OrderDTO.
        /// </summary>
        /// <param name="orderDTO">Thông tin đơn hàng</param>
        /// <param name="userId">ID người dùng</param>
        /// <returns>OrderDTO đã được tạo</returns>
        Task<OrderDTO> CreateOrderAsync(OrderDTO orderDTO, string userId);

        /// <summary>
        /// Cập nhật trạng thái đơn hàng.
        /// </summary>
        /// <param name="id">ID của đơn hàng</param>
        /// <param name="status">Trạng thái mới của đơn hàng</param>
        /// <returns>True nếu cập nhật thành công, False nếu không tìm thấy đơn hàng</returns>
        Task<bool> UpdateOrderStatusAsync(Guid id, string status);

        /// <summary>
        /// Tạo đơn hàng từ giỏ hàng của người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>OrderDTO đã được tạo từ giỏ hàng</returns>
        Task<OrderDTO> CreateOrderFromCartAsync(string userId);
    }
}
