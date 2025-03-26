using KeyBoard.Data;
using KeyBoard.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KeyBoard.Repositories.Implementations
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly ApplicationDbContext _context;

        public HoaDonRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<int> CreateHoaDonFromOrderAsync(Guid orderId)
        {
                var order = await _context.Orders
         .Include(o => o.OrderDetails)
         .ThenInclude(od => od.Product) // Đảm bảo load Product
         .FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null) throw new Exception("Order not found");
            var hoadon = new HoaDon
            {
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
            _context.HoaDons.Add(hoadon);
            await _context.SaveChangesAsync();
            return hoadon.MaHd;
        }

        public async Task<HoaDon?> GetHoaDonByIdAsync(int maHd)
        {
            return await _context.HoaDons
                .Include(hd => hd.ChiTietHoaDons)
                .Include(hd => hd.MaTrangThaiNavigation)
                .FirstOrDefaultAsync(hd => hd.MaHd == maHd);
        }

        public async Task<IEnumerable<HoaDon>> GetHoaDonsByUserAsync(string userId)
        {
                return await _context.HoaDons
            .Where(hd => hd.UserId == userId)
            .Include(hd => hd.ChiTietHoaDons) // Lấy chi tiết hóa đơn
            .Include(hd => hd.MaTrangThaiNavigation) // Lấy trạng thái
            .OrderByDescending(hd => hd.NgayDat) // Sắp xếp mới nhất lên trước
            .ToListAsync();
        }

        public async Task<bool> UpdatePaymentStatusAsync(int maHd, int trangThai, DateTime ngayGiao)
        {
            var existingHoaDon = await _context.HoaDons.FindAsync(maHd);
            if (existingHoaDon == null) return false;

            // Kiểm tra xem hóa đơn có đúng với OrderId không (nếu cần thiết)
            existingHoaDon.MaTrangThai = trangThai;
            existingHoaDon.NgayGiao = ngayGiao;

            _context.HoaDons.Update(existingHoaDon);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
