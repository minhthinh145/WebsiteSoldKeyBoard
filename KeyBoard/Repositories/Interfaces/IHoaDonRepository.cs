using KeyBoard.Data;

namespace KeyBoard.Repositories.Interfaces
{
    public interface IHoaDonRepository
    {
        // Lấy hóa đơn theo ID
        Task<Bill?> GetHoaDonByIdAsync(int maHd);

        // Lấy danh sách hóa đơn của một User
        Task<IEnumerable<Bill>> GetHoaDonsByUserAsync(string userId);

        // Cập nhật trạng thái thanh toán
        Task<bool> UpdatePaymentStatusAsync(int maHd, int trangThai, DateTime ngayGiao);

        // Xóa hóa đơn theo mã hóa đơn
        Task<bool> DeleteHoaDonByIdAsync(int maHd);

        // Lấy đơn hàng từ database theo orderId (để Service xử lý logic)
        Task<Order?> GetOrderWithDetailsAsync(Guid orderId);

        // Thêm hóa đơn vào database
        Task<int> AddHoaDonAsync(Bill hoadon);
    }
}
