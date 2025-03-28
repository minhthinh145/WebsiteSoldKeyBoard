using AutoMapper;
using KeyBoard.Data;
using KeyBoard.DTOs.HoaDonsDTOs;
using KeyBoard.Repositories.Interfaces;
using KeyBoard.Services.Interfaces;

namespace KeyBoard.Services.Implementations
{
    public class HoaDonService : IHoaDonService
    {
        private readonly IHoaDonRepository _repo;
        private readonly IMapper _mapper;

        public HoaDonService(IHoaDonRepository repo , IMapper mapper )
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<int> CreateHoaDonFromOrderAsync(Guid orderId)
        {
            var order = await _repo.GetOrderWithDetailsAsync(orderId);
            if (order == null) 
            {
                throw new Exception("Order not found");
            }
            var hoadon = new HoaDon { 
                UserId = order.UserId,
                NgayDat = DateTime.UtcNow,
                MaTrangThai = 1,
                DiaChi = "Chưa cập nhật",
                CachThanhToan = "VNPay",
                CachVanChuyen = "Giao hàng tiết kiệm",
                PhiVanChuyen = 20000,
                ChiTietHoaDons = order.OrderDetails.Select(od => new ChiTietHoaDon
                {
                    TenHh = od.Product.Name,
                    MaHh = od.ProductId,
                    SoLuong = od.Quantity,
                    DonGia = od.UnitPrice
                }).ToList()
            };
            await _repo.AddHoaDonAsync(hoadon);
            return hoadon.MaHd;
        }

            public async Task<bool> DeleteHoaDonByIdAsync(int maHd)
            {
                await GetHoaDonByIdAsync(maHd);
                return await _repo.DeleteHoaDonByIdAsync(maHd);
            }

        public async Task<HoaDonDTO> GetHoaDonByIdAsync(int id)
        {
            var HoaDon = await _repo.GetHoaDonByIdAsync(id);
            if (HoaDon == null)
            {
                throw new KeyNotFoundException("Không tìm thấy hóa đơn này.");
            }
            return _mapper.Map<HoaDonDTO>(HoaDon);
        }

        public async Task<List<HoaDonDTO>> GetHoaDonsByUserIdAsync(string userId)
        {
            var hoadons = await _repo.GetHoaDonsByUserAsync(userId) ?? new List<HoaDon>();

            if (!hoadons.Any())
            {
                throw new KeyNotFoundException("Không tìm thấy hóa đơn cho người dùng này.");
            }

            return _mapper.Map<List<HoaDonDTO>>(hoadons);
        }

        public async Task<bool> UpdatePaymentStatusAsync(int maHd, int status, DateTime updatedAt)
        {
            var hoaDon = await GetHoaDonByIdAsync(maHd);
            if (hoaDon == null) 
            {
                throw new KeyNotFoundException("Không tìm thấy hóa đơn này.");
            }
            return await _repo.UpdatePaymentStatusAsync(maHd, status, updatedAt);
        }
    }
}
