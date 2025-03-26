using KeyBoard.Data;

namespace KeyBoard.Services.Interfaces
{
    public interface IHoaDonService
    {
        Task<int> CreateHoaDonFromOrderAsync(Guid orderId);
        Task<HoaDon?> GetHoaDonByIdAsync(int maHd);
        Task<List<HoaDon>> GetHoaDonsByUserIdAsync(string userId);
        Task<bool> UpdateHoaDonStatusAsync(int maHd, int maTrangThai , DateTime ngaygiao);
    }
}
