using KeyBoard.DTOs.HoaDonsDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeyBoard.Services.Interfaces
{
    /// <summary>
    /// Interface định nghĩa các phương thức xử lý logic cho hóa đơn.
    /// </summary>
    public interface IHoaDonService
    {
        /// <summary>
        /// Lấy danh sách hóa đơn theo User ID.
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Danh sách hóa đơn</returns>
        Task<List<HoaDonDTO>> GetHoaDonsByUserIdAsync(string userId);

        /// <summary>
        /// Lấy thông tin hóa đơn theo ID.
        /// </summary>
        /// <param name="id">ID của hóa đơn</param>
        /// <returns>Thông tin hóa đơn</returns>
        Task<HoaDonDTO> GetHoaDonByIdAsync(int id);

        /// <summary>
        /// Tạo hóa đơn từ Order ID.
        /// </summary>
        /// <param name="orderId">ID của đơn hàng</param>
        /// <returns>Mã hóa đơn được tạo</returns>
        Task<int> CreateHoaDonFromOrderAsync(Guid orderId);

        /// <summary>
        /// Cập nhật trạng thái thanh toán của hóa đơn.
        /// </summary>
        /// <param name="maHd">Mã hóa đơn</param>
        /// <param name="status">Trạng thái thanh toán</param>
        /// <param name="updatedAt">Thời gian cập nhật</param>
        /// <returns>Kết quả cập nhật (true nếu thành công, ngược lại false)</returns>
        Task<bool> UpdatePaymentStatusAsync(int maHd, int status, DateTime updatedAt);

        /// <summary>
        /// Xóa hóa đơn theo mã hóa đơn.
        /// </summary>
        /// <param name="maHd">Mã hóa đơn</param>
        /// <returns>Kết quả xóa (true nếu thành công, ngược lại false)</returns>
        Task<bool> DeleteHoaDonByIdAsync(int maHd);
    }
}
