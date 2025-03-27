using KeyBoard.Data;

namespace KeyBoard.Repositories.Interfaces
{
    public interface IHoaDonRepository
    {
        Task<int> CreateHoaDonFromOrderAsync(Guid orderId);
        Task<HoaDon?> GetHoaDonByIdAsync(int maHd);
        Task<IEnumerable<HoaDon>> GetHoaDonsByUserAsync(string userId);
        Task<bool> UpdatePaymentStatusAsync(int maHd, int trangThai, DateTime ngaygiao);
        Task<bool> DeleteHoaDonByIdAsync(int maHD);
    }
}
