using KeyBoard.Data;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.Interfaces;

namespace KeyBoard.Services.Implementations
{
    public class HoaDonService : IHoaDonService
    {
        private readonly IHoaDonRepository _repo;

        public HoaDonService(IHoaDonRepository repo )
        {
            _repo = repo;
        }
        public Task<int> CreateHoaDonFromOrderAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<HoaDon?> GetHoaDonByIdAsync(int maHd)
        {
            throw new NotImplementedException();
        }

        public Task<List<HoaDon>> GetHoaDonsByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateHoaDonStatusAsync(int maHd, int maTrangThai, DateTime ngaygiao)
        {
            throw new NotImplementedException();
        }
    }
}
