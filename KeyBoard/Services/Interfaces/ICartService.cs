using KeyBoard.DTOs;
using KeyBoard.Helpers;

namespace KeyBoard.Services.Interfaces
{
    public interface ICartService
    {
        /// <summary>
        /// Lấy danh sách sản phẩm trong giỏ hàng của người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <returns>Danh sách sản phẩm trong giỏ hàng.</returns>
        Task<CartDTO> GetCartItemsAsync(string userId);

        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng.
        /// </summary>
        /// <param name="cartDTO">Thông tin sản phẩm cần thêm.</param>
        /// <returns>Trả về true nếu thêm thành công.</returns>
        Task<ServiceResult> AddToCartAsync(string userId,AddToCartDTO cartDTO);

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong giỏ hàng.
        /// </summary>
        /// <param name="cartDTO">Thông tin sản phẩm cần cập nhật.</param>
        /// <returns>Trả về true nếu cập nhật thành công.</returns>
        Task<bool> UpdateCartAsync(CartItemDTO cartDTO, string userId);

        /// <summary>
        /// Xóa một sản phẩm khỏi giỏ hàng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <param name="productId">ID của sản phẩm cần xóa.</param>
        /// <returns>Trả về true nếu xóa thành công.</returns>
        Task<bool> RemoveFromCartAsync(string userId, Guid productId);

        /// <summary>
        /// Xóa toàn bộ sản phẩm trong giỏ hàng của người dùng.
        /// </summary>
        /// <param name="userId">ID của người dùng.</param>
        /// <returns>Trả về true nếu xóa thành công.</returns>
        Task<bool> ClearCartAsync(string userId);
    }
}
