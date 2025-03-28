using KeyBoard.DTOs.HoaDonsDTOs;

namespace KeyBoard.Services.Interfaces
{
    public interface IChiTietHoaDonService
    {
        /// <summary>
        /// Lấy danh sách chi tiết hóa đơn theo mã hóa đơn (maHd)
        /// </summary>
        /// <param name="maHd">Mã hóa đơn cần lấy chi tiết</param>
        /// <returns>Danh sách các chi tiết hóa đơn</returns>
        Task<List<ChiTietHoaDonDTO>> GetChiTietHoaDonsByHoaDonIdAsync(int maHd);

        /// <summary>
        /// Cập nhật số lượng sản phẩm trong một chi tiết hóa đơn
        /// </summary>
        /// <param name="id">Mã chi tiết hóa đơn cần cập nhật</param>
        /// <param name="newQuantity">Số lượng mới</param>
        /// <param name="newPrice">Đơn giá mới</param>
        /// <returns>Trả về `true` nếu cập nhật thành công, `false` nếu thất bại</returns>
        Task<bool> UpdateChiTietHoaDonAsync(int id, int? newQuantity, decimal? newPrice);

        /// <summary>
        /// Xóa một chi tiết hóa đơn theo ID nếu hóa đơn chưa thanh toán
        /// </summary>
        /// <param name="id">Mã chi tiết hóa đơn cần xóa</param>
        /// <returns>Trả về `true` nếu xóa thành công, `false` nếu thất bại</returns>
        Task<bool> DeleteChiTietHoaDonAsync(int id);

        /// <summary>
        /// Lấy  chi tiết hóa đơn theo mã chi tiết hóa đơn (maHd)
        /// </summary>
        /// <param name="maChiTietHD">Mã chi tiet hóa đơn cần lấy </param>
        /// <returns> chi tiết hóa đơn</returns>
        Task<ChiTietHoaDonDTO> GetChiTietHoaDonsByChiTietHoaDonIdAsync(int maChiTietHD);
    }
}
